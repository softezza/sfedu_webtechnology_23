using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProgramValidation
{
    using CreatorsExe;
    using System.Globalization;
    using Utils;

    public class Validator
    {
        CreatorExeCpp _creatorCpp;
        Comparator _comparator;

        public Validator()
        {
            _creatorCpp = new CreatorExeCpp();
            _comparator = new Comparator();
        }

        public async Task<bool> ValidateCodeCpp(string code, DataSet[] sets, double accuracy = 0.001f)
        {
            var exeFile = await _creatorCpp.CreateExeFile(code);
            if (string.IsNullOrEmpty(exeFile)) return false;

            var result = await ValidateExe(exeFile, sets, accuracy);
            return result;
        }

        public async Task<bool> ValidateExe(string exeFile, DataSet[] sets, double accuracy = 0.001f)
        {
            var pInfo = new ProcessStartInfo()
            {

                FileName = exeFile,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
            };

            for (int i = 0; i < sets.Length; i++)
                if (!await RunData(pInfo, sets[i], i + 1, accuracy)) return false;

            return true;
        }

        private async Task<bool> RunData(ProcessStartInfo info, DataSet set, int number, double accuracy)
        {
            if (set.ExpectedOutputData == null || set.ExpectedOutputData.Count == 0)
                throw new EmptyOrNullReferenceExpectedOutputDataException(number);

            if (set.InputData == null) throw new NullReferenceExpectedInputDataException(number);

            var expectedOutputData = set.ExpectedOutputData;
            var inputData = set.InputData;

            var process = Process.Start(info);
            if (process == null) throw new ExeNotFoundException(info.FileName);

            if (inputData.Count != 0)
            {
                using (var writer = process.StandardInput)
                {
                    for (int i = 0; i < inputData.Count; i++)
                    {
                        var input = inputData[i];

                        if (input is UnitDouble unitDouble)
                        {
                            var value = unitDouble.Value;
                            writer.Write(ConvertDoubleToString(value));
                        }
                        else writer.Write(input.Value);

                        writer.Write((char)13);
                    }
                }
            }

            using (var reader = process.StandardOutput)
            {
                for (int i = 0; i < expectedOutputData.Count; i++)
                {
                    var line = reader.ReadLine();

                    if (!_comparator.Compare(expectedOutputData[i], line, accuracy)) return false;
                }
            }

            await process.WaitForExitAsync();
            return true;
        }

        private string ConvertDoubleToString(double value)
        {
            var nfi = new NumberFormatInfo(); new NumberFormatInfo() { NumberDecimalSeparator = "." };
            nfi.NumberDecimalSeparator = ".";

            return value.ToString(nfi);
        }

    }

    class Comparator
    {
        static Dictionary<Type, Func<IUnit, string, bool>> _comparisonMethods = new Dictionary<Type, Func<IUnit, string, bool>>()
        {
            [typeof(UnitInt)] = (unit, line) => Compare((UnitInt)unit, line),
            [typeof(UnitChar)] = (unit, line) => Compare((UnitChar)unit, line),
            [typeof(UnitString)] = (unit, line) => Compare((UnitString)unit, line),
        };

        public bool Compare(IUnit expectedUnit, string line, double accuracy)
        {
            if (expectedUnit is UnitDouble unitDouble) return Compare(unitDouble, line, accuracy);
            else return _comparisonMethods[expectedUnit.GetType()](expectedUnit, line);
        }

        private static bool Compare(UnitInt expectedUnit, string line)
        {
            var expectedValue = expectedUnit.Value;

            try
            {
                var realValue = Convert.ToInt32(line);
                return realValue == expectedValue;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool Compare(UnitDouble expectedUnit, string line, double accuracy)
        {
            var expectedValue = expectedUnit.Value;

            try
            {
                var realValue = double.Parse(line, CultureInfo.InvariantCulture);

                var difference = Math.Abs((double)expectedValue - (double)realValue);
                if (difference <= accuracy) return true;
                else return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool Compare(UnitChar expectedUnit, string line)
        {
            var expectedValue = expectedUnit.Value;

            try
            {
                var realValue = Convert.ToChar(line);
                return realValue == expectedValue;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static bool Compare(UnitString expectedUnit, string line)
        {
            var expectedValue = expectedUnit.Value;
            return string.Equals(expectedValue, line);
        }
    }
}