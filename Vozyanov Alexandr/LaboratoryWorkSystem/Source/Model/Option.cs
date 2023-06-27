using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MessagePack;
using MessagePack.Formatters;
using MsgPack.Serialization;

namespace LaboratoryWorkSystem
{
    public interface IViewOption
    {
        int Number { get; }
        IReadOnlyList<IViewTask> Tasks { get; }
    }

    [MessagePackFormatter(typeof(Option.Formatter))]
    public class Option : IViewOption
    {
        private int number;
        private List<Task> _tasks;


        public int Number { get => number; set => number = value; }
        public List<Task> Tasks => _tasks;

        IReadOnlyList<IViewTask> IViewOption.Tasks => _tasks;

        public Option(int number)
        {
            Number = number;
            _tasks = new List<Task>();
        }

        private Option(int number, List<Task> tasks)
        {
            Number = number;
            _tasks = tasks;
        }


        class Formatter : IMessagePackFormatter<Option>
        {
            public void Serialize(ref MessagePackWriter writer, Option option, MessagePackSerializerOptions options)
            {
                writer.WriteMapHeader(2);
                writer.Write("Number"); writer.Write(option.number);

                writer.Write("Tasks");
                options.Resolver.GetFormatterWithVerify<List<Task>>().Serialize(ref writer, option._tasks, options);
            }

            public Option Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                try
                {
                    if (reader.TryReadNil())
                        throw new FormattingException("Файл не имеет экземпляра Option");

                    options.Security.DepthStep(ref reader);

                    var count = reader.ReadMapHeader();
                    if (count != 2) throw SerializationExceptions.NewUnexpectedArrayLength(2, count);

                    int number = 0;
                    List<Task> tasks = null;

                    for (int i = 0; i < count; i++)
                    {
                        var key = reader.ReadString();

                        switch (key)
                        {
                            case "Number":
                                number = reader.ReadInt32();
                                break;
                            case "Tasks":
                                tasks = options.Resolver
                                    .GetFormatterWithVerify<List<Task>>()
                                    .Deserialize(ref reader, options);
                                break; 
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    if (number <= 0)
                        throw new FormattingException("Файл не хранит ключ Number или хранит значение меньше или равно нулю.");

                    if (tasks == null)
                        throw new FormattingException("Файл не хранит ключ Tasks или хранит null-значение.");

                    return new Option(number, tasks);
                }
                catch (IOException e)
                {
                    throw new CorruptedFileException(e.Message);
                }
                finally
                {
                    reader.Depth--;
                }
            }
        }
    }
}
