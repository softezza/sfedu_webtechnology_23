using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MessagePack;
using System.Runtime.Serialization;
using MessagePack.Formatters;
using MsgPack.Serialization;
using MessagePackSerializer = MessagePack.MessagePackSerializer;

namespace LaboratoryWorkSystem
{
    public interface IViewLaboratoryWork
    {
        string Name { get; }
        string Description { get; }
        IReadOnlyList<IViewOption> Options { get; }
    }

    [MessagePackFormatter(typeof(Formatter))]
    public class LaboratoryWork : IViewLaboratoryWork
    {
        private string name;
        private string description;
        private List<Option> _options;

        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public List<Option> Options => _options;

        IReadOnlyList<IViewOption> IViewLaboratoryWork.Options => _options;

        public LaboratoryWork(string name, string description = "")
        {
            Name = name;
            Description = description;

            _options = new List<Option>();
        }

        private LaboratoryWork(string name, string description, List<Option> options) 
        {
            Name = name;
            Description = description;
            _options = options;
        }

        public class FileHelper
        {
            public static readonly string Extension = ".lw";

            FileInfo _info;
            FileStream _stream;

            public FileInfo FileInfo => _info;

            public async System.Threading.Tasks.Task Save(LaboratoryWork labWork)
            {
                _stream.Close();
                _stream = new FileStream(_info.FullName, FileMode.Create, FileAccess.Write | FileAccess.Read);

                await MessagePackSerializer.SerializeAsync(_stream, labWork);
            }

            public void Close()
            {
                _stream.Close();
                _stream.Dispose();
            }

            public static async Task<FileHelper> CreateFile(LaboratoryWork labWork, string directoryPath, bool overWrite = false)
            {
                var fInfo = new FileInfo(directoryPath + @"\" + labWork.Name + Extension);

                if (!overWrite && fInfo.Exists)
                    throw new FileExistenceException(fInfo.FullName, FileExistenceException.Type.Overwriting);

                var helper = new FileHelper();
                helper._stream = new FileStream(fInfo.FullName, FileMode.Create, FileAccess.Write | FileAccess.Read);
                helper._info = fInfo;

                await MessagePackSerializer.SerializeAsync(helper._stream, labWork);

                return helper;
            }

            public static async Task<(LaboratoryWork, FileHelper)> OpenFile(string fileName)
            {
                var fileInfo = new FileInfo(fileName);

                if (fileInfo.Extension != Extension) throw new ExtensionException(fileInfo.Extension, Extension);
                if (!File.Exists(fileInfo.FullName))
                    throw new FileExistenceException(fileInfo.FullName, FileExistenceException.Type.NotExists);

                var helper = new FileHelper();

                helper._stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Write | FileAccess.Read);
                helper._info = fileInfo;

                var labWork = await MessagePackSerializer.DeserializeAsync<LaboratoryWork>(helper._stream);

                return (labWork, helper);
            }

            public static async Task<IViewLaboratoryWork> LoadLaboratoryWork(string fileName)
            {
                var fileInfo = new FileInfo(fileName);

                if (fileInfo.Extension != Extension) throw new ExtensionException(fileInfo.Extension, Extension);
                if (!File.Exists(fileInfo.FullName))
                    throw new FileExistenceException(fileInfo.FullName, FileExistenceException.Type.NotExists);

                using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Write | FileAccess.Read))
                {
                    return await MessagePackSerializer.DeserializeAsync<LaboratoryWork>(stream);
                }
            }
        }

        class Formatter : IMessagePackFormatter<LaboratoryWork>
        {
            public void Serialize(ref MessagePackWriter writer, LaboratoryWork labWork, MessagePackSerializerOptions options)
            {
                writer.WriteMapHeader(3);
                
                writer.Write("Name"); writer.Write(labWork.name);

                writer.Write("Description"); writer.Write(labWork.Description);
                
                writer.Write("Options");
                options.Resolver.GetFormatterWithVerify<List<Option>>()
                    .Serialize(ref writer, labWork._options, options);
            }

            public LaboratoryWork Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions mpsOptions)
            {
                try
                {
                    if (reader.TryReadNil())
                        throw new FormattingException("Файл не имеет экземпляра LaboratoryWork");

                    mpsOptions.Security.DepthStep(ref reader);

                    var count = reader.ReadMapHeader();
                    if (count != 3) throw SerializationExceptions.NewUnexpectedArrayLength(3, count);

                    string name = null;
                    string description = null;
                    List<Option> options = null;

                    for (int i = 0; i < count; i++)
                    {
                        var key = reader.ReadString();

                        switch (key)
                        {
                            case "Name":
                                name = reader.ReadString();
                                break;
                            case "Description":
                                description = reader.ReadString();
                                break;
                            case "Options":
                                options = mpsOptions.Resolver
                                    .GetFormatterWithVerify<List<Option>>()
                                    .Deserialize(ref reader, mpsOptions);
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    if (name == null)
                        throw new FormattingException("Файл не хранит ключ Name или хранит null-значение.");

                    if (name == string.Empty)
                        throw new FormattingException("Файл не может содержать пустое значение Name.");

                    if (description == null)
                        throw new FormattingException("Файл не хранит ключ Description или хранит null-значение.");

                    if (options == null)
                        throw new FormattingException("Файл не хранит ключ Options или хранит null-значение.");

                    return new LaboratoryWork(name, description, options);
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
