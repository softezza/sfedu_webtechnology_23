using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramValidation
{
    public class DataSet : ICloneable
    {
        public List<IUnit> InputData { get; }
        public List<IUnit> ExpectedOutputData { get; }

        public DataSet(List<IUnit> inputData, List<IUnit> expectedOutputData)
        {
            InputData = inputData;
            ExpectedOutputData = expectedOutputData;
        }

        public object Clone()
        {
            var inputData = new List<IUnit>(InputData);
            var expectedOutputData = new List<IUnit>(ExpectedOutputData);

            return new DataSet(inputData, expectedOutputData);
        }
    }

    public interface IUnit
    {
        object Value { get; }
    }

    public struct UnitInt : IUnit
    {
        public int Value;
        object IUnit.Value => Value;

        public UnitInt(int value) => Value = value;
    }

    public struct UnitChar : IUnit
    {
        public char Value;
        object IUnit.Value => Value;

        public UnitChar(char value) => Value = value;
    }

    public struct UnitDouble : IUnit
    {
        public double Value;
        object IUnit.Value => Value;

        public UnitDouble(double value) => Value = value;
    }

    public struct UnitString : IUnit
    {
        public string Value;
        object IUnit.Value => Value;

        public UnitString(string value) => Value = value;
    }
}
