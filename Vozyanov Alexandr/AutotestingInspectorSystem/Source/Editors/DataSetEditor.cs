using System.Collections.Generic;
using ProgramValidation;

namespace AutotestingInspectorSystem
{
    public class DataSetEditor
    {
        DataSet _set;

        InputDataEditor _inputEditor;
        ExpectedOutputDataEditor _outputEditor;

        public IDataEditor InputEditor => _inputEditor;
        public IDataEditor OutputEditor => _outputEditor;

        public DataSetEditor(DataSet set)
        {
            _set = set;

            _inputEditor = new InputDataEditor(_set.InputData);
            _outputEditor = new ExpectedOutputDataEditor(_set.ExpectedOutputData);
        }

        public interface IDataEditor
        {
            IUnit CreateUnit();
            void RemoveUnit(int index);
            void UpdateUnit(int index, IUnit unit);
        }

        public class InputDataEditor : IDataEditor
        {
            List<IUnit> _inputData;
            public IReadOnlyList<IUnit> InputData => _inputData;

            public InputDataEditor(List<IUnit> inputData) => _inputData = inputData;

            public IUnit CreateUnit()
            {
                var unit = new UnitInt(0);
                _inputData.Add(unit);

                return unit;
            }

            public void RemoveUnit(int index)
            {
                _inputData.RemoveAt(index);
            }

            public void UpdateUnit(int index, IUnit unit)
            {
                _inputData[index] = unit;
            }
        }

        public class ExpectedOutputDataEditor : IDataEditor
        {
            List<IUnit> _expectedOutputData;
            public IReadOnlyList<IUnit> ExpectedOutputData => _expectedOutputData;

            public ExpectedOutputDataEditor(List<IUnit> expectedOutputData) => _expectedOutputData = expectedOutputData;

            public IUnit CreateUnit()
            {
                var unit = new UnitInt(0);
                _expectedOutputData.Add(unit);

                return unit;
            }

            public void RemoveUnit(int index)
            {
                _expectedOutputData.RemoveAt(index);
            }

            public void UpdateUnit(int index, IUnit unit)
            {
                _expectedOutputData[index] = unit;
            }
        }
    }
}
