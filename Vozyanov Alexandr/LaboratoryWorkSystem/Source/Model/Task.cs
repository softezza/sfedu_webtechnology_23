using System;
using System.Collections.Generic;
using ProgramValidation;
using MessagePack;
using MessagePack.Formatters;
using MsgPack.Serialization;
using MessagePack.Resolvers;
using MessagePackSerializer = MessagePack.MessagePackSerializer;
using System.IO;
using System.Collections;

namespace LaboratoryWorkSystem
{
    public interface IViewTask : IEnumerable<DataSet>
    {
        int Number { get; }
        string Description { get; }

        List<DataSet> GetDataSets();
        DataSet GetDataSet(int index);
    }

    [MessagePackFormatter(typeof(Task.Formatter))]
    public class Task : IViewTask, ICloneable
    {
        int number;
        string description;

        List<DataSet> _dataSets;

        public int Number { get => number; set => number = value; }
        public string Description { get => description; set => description = value; }
        public List<DataSet> DataSets => _dataSets;

        public Task(int number)
        {
            Number = number;
            Description = "";

            _dataSets = new List<DataSet>();
        }

        private Task(int number, string description, List<DataSet> sets)
        {
            Number = number;
            Description = description;
            _dataSets = sets;
        }

        public List<DataSet> GetDataSets()
        {
            var dataSetsClone = new List<DataSet>();
            foreach (var set in _dataSets)
            {
                var setClone = set.Clone() as DataSet;
                dataSetsClone.Add(setClone);
            }

            return dataSetsClone;
        }
        public DataSet GetDataSet(int index) => _dataSets[index].Clone() as DataSet;

        public IEnumerator<DataSet> GetEnumerator()
        {
            foreach (var set in _dataSets)
                yield return set.Clone() as DataSet;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public object Clone() => new Task(Number, Description, GetDataSets());

        public void Update(Task task)
        {
            Description = task.Description;
            _dataSets = task.GetDataSets();
        }



        class Formatter : IMessagePackFormatter<Task>
        {
            public void Serialize(ref MessagePackWriter writer, Task task, MessagePackSerializerOptions options)
            {
                writer.WriteMapHeader(3);
                writer.Write("Number"); writer.Write(task.number);
                writer.Write("Description"); writer.Write(task.description);

                var resolvers = CompositeResolver.Create(new DataSetFormatter());

                writer.Write("DataSets");
                options.Resolver.GetFormatterWithVerify<List<DataSet>>()
                    .Serialize(ref writer, task._dataSets, options.WithResolver(resolvers));
            }

            public Task Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                try
                {
                    if (reader.TryReadNil())
                        throw new FormattingException("Файл не имеет экземпляра Tasks");

                    options.Security.DepthStep(ref reader);

                    var count = reader.ReadMapHeader();
                    if (count != 3) throw SerializationExceptions.NewUnexpectedArrayLength(3, count);

                    var number = 0;
                    string description = null;
                    List<DataSet> dataSets = null;

                    for (int i = 0; i < count; i++)
                    {
                        var key = reader.ReadString();

                        switch (key)
                        {
                            case "Number":
                                number = reader.ReadInt32();
                                break;
                            case "Description":
                                description = reader.ReadString();
                                break;
                            case "DataSets":
                                var resolver = CompositeResolver.Create(new DataSetFormatter());
                                dataSets = options.Resolver
                                    .GetFormatterWithVerify<List<DataSet>>()
                                    .Deserialize(ref reader, options.WithResolver(resolver));
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }


                    if (number <= 0)
                        throw new FormattingException("Файл не хранит ключ Number или хранит значение меньше или равно нулю.");

                    if (description == null)
                        throw new FormattingException("Файл не хранит ключ Description или хранит null-значение.");

                    if (dataSets == null)
                        throw new FormattingException("Файл не хранит ключ DataSets или хранит null-значение.");

                    return new Task(number, description, dataSets);
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
        class DataSetFormatter : IMessagePackFormatter<DataSet>
        {
            public void Serialize(ref MessagePackWriter writer, DataSet set, MessagePackSerializerOptions options)
            {
                var inputData = set.InputData.ConvertAll(unit => PackagingUnit.Pack(unit));
                var expectedOutputData = set.ExpectedOutputData.ConvertAll(unit => PackagingUnit.Pack(unit));

                writer.WriteMapHeader(2);

                writer.Write("InputData");
                MessagePackSerializer.Serialize(typeof(List<PackagingUnit>), 
                    ref writer, inputData,
                    MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolverAllowPrivate.Instance));

                writer.Write("ExpectedOutputData");
                MessagePackSerializer.Serialize(typeof(List<PackagingUnit>), 
                    ref writer, expectedOutputData,
                    MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolverAllowPrivate.Instance));
            }

            public DataSet Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
            {
                try
                {
                    if (reader.TryReadNil())
                        throw new FormattingException("Файл не имеет экземпляра DataSet");

                    options.Security.DepthStep(ref reader);

                    var count = reader.ReadMapHeader();
                    if (count != 2) throw SerializationExceptions.NewUnexpectedArrayLength(2, count);

                    List<PackagingUnit> inputData = null;
                    List<PackagingUnit> expectedOutputData = null;

                    for (int i = 0; i < count; i++)
                    {
                        var key = reader.ReadString();

                        switch (key)
                        {
                            case "InputData":
                                inputData = MessagePackSerializer
                                    .Deserialize(typeof(List<PackagingUnit>), ref reader,
                                    MessagePackSerializerOptions.Standard
                                    .WithResolver(ContractlessStandardResolverAllowPrivate.Instance)) as List<PackagingUnit>;
                                break;
                            case "ExpectedOutputData":
                                expectedOutputData = MessagePackSerializer
                                   .Deserialize(typeof(List<PackagingUnit>), ref reader,
                                   MessagePackSerializerOptions.Standard
                                   .WithResolver(ContractlessStandardResolverAllowPrivate.Instance)) as List<PackagingUnit>;
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }

                    if (inputData == null)
                        throw new FormattingException("Файл не хранит ключ InputData или хранит null-значение.");

                    if (expectedOutputData == null)
                        throw new FormattingException("Файл не хранит ключ ExpectedOutputData или хранит null-значение.");


                    return new DataSet(inputData.ConvertAll(packaging => PackagingUnit.Unpack(packaging)),
                        expectedOutputData.ConvertAll(packaging => PackagingUnit.Unpack(packaging)));

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

        [MessagePackObject]
        struct PackagingUnit
        {
            [Key(0)]
            public Type TypeValue;
            [Key(1)]
            public object Value;

            public static PackagingUnit Pack(IUnit unit)
            {
                switch (unit)
                {
                    case UnitInt unitInt:
                        return new PackagingUnit() { TypeValue = Type.Int, Value = unitInt.Value };

                    case UnitDouble unitDouble:
                        return new PackagingUnit() { TypeValue = Type.Double, Value = unitDouble.Value };

                    case UnitChar unitChar:
                        return new PackagingUnit() { TypeValue = Type.Char, Value = unitChar.Value };

                    case UnitString unitString:
                        return new PackagingUnit() { TypeValue = Type.String, Value = unitString.Value };
                    default:
                        throw new InvalidCastException($"Передан неподдерживаемый тип IUnit: {unit.GetType()}.");
                }
            }

            public static IUnit Unpack(PackagingUnit packaging)
            {
                switch (packaging.TypeValue)
                {
                    case Type.Int: return new UnitInt((int)packaging.Value);
                    case Type.Double: return new UnitDouble((double)packaging.Value);
                    case Type.Char: return new UnitChar(Convert.ToChar(packaging.Value));
                    case Type.String: return new UnitString((string)packaging.Value);
                    default: 
                        throw new InvalidCastException($"Передан неподдерживаемый тип PackaginUnit: {packaging.TypeValue}.");
                }
            }

            public enum Type
            {
                Int,
                Double,
                Char,
                String
            }
        }
    }
}
