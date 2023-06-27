using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LaboratoryWorkSystem;
using ProgramValidation;
using System.Reflection;

namespace LaboratoryWorkSystem.Tests
{

    [TestClass()]
    public class SerilizerTests
    {
        [TestMethod()]
        public void SerializeTest()
        {
            AsyncStart();
        }

        private async void AsyncStart()
        {
            //var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            //var labWork = CreateLabWork();
            //var path = await LaboratoryWork.FileHelper.CreateFile(labWork, directory, true);
            //var result = await LaboratoryWork.FileHelper.Load(path);

            //AssertLabWorks(labWork, result as LaboratoryWork);
        }

        private void AssertLabWorks(LaboratoryWork expectedLabWork, LaboratoryWork realLabWork)
        {
            Assert.AreEqual(expectedLabWork.Name, realLabWork.Name);
            Assert.AreEqual(expectedLabWork.Description, realLabWork.Description);
            Assert.AreEqual(expectedLabWork.Options.Count, expectedLabWork.Options.Count);

            for (int i = 0; i < expectedLabWork.Options.Count; i++)
                AssertOption(expectedLabWork.Options[i], realLabWork.Options[i]);
        }

        private void AssertOption(Option expectedOption, Option realOption)
        {
            Assert.AreEqual(expectedOption.Number, realOption.Number);
            Assert.AreEqual(expectedOption.Tasks.Count, realOption.Tasks.Count);

            for (int i = 0; i < expectedOption.Tasks.Count; i++)
                AssertTask(expectedOption.Tasks[i], realOption.Tasks[i]);
        }

        private void AssertTask(Task expectedTask, Task realTask)
        {
            Assert.AreEqual(expectedTask.Number, realTask.Number);
            Assert.AreEqual(expectedTask.Description, realTask.Description);
            Assert.AreEqual(expectedTask.DataSets.Count, realTask.DataSets.Count);

            for (int i = 0; i < expectedTask.DataSets.Count; i++) 
                AssertDataSet(expectedTask.DataSets[i], realTask.DataSets[i]);
        }

        private void AssertDataSet(DataSet expectedSet, DataSet realSet)
        {
            AssertListUnits(expectedSet.InputData, realSet.InputData);
            AssertListUnits(expectedSet.ExpectedOutputData, realSet.ExpectedOutputData);
        }

        private void AssertListUnits(List<IUnit> expectedUnits, List<IUnit> realUnits)
        {
            Assert.AreEqual(expectedUnits.Count, realUnits.Count);
            for (int i = 0; i < expectedUnits.Count; i++) 
                AssertUnit(expectedUnits[i], realUnits[i]);
        }

        private void AssertUnit(IUnit expectedUnit, IUnit realUnit)
        {
            Assert.IsInstanceOfType(realUnit, expectedUnit.GetType());
            Assert.AreEqual(expectedUnit.Value, realUnit.Value);
        }

        private LaboratoryWork CreateLabWork()
        {
            var labWork = new LaboratoryWork("Лабораторная работа №1");

            var random = new Random();
            var countOptions = random.Next(1, 5);

            for (int i = 1; i <= countOptions; i++)
            {
                var option = CreatOption(i);
                labWork.Options.Add(option);
            }

            return labWork;
        }

        private Option CreatOption(int number)
        {
            var option = new Option(number);

            var random = new Random();
            var countTasks = random.Next(1, 5);

            for (int i = 1; i <= countTasks; i++)
            {
                var task = CreateTask(number);
                option.Tasks.Add(task);
            }

            return option;
        }

        private Task CreateTask(int number)
        {
            var task = new Task(number);

            var random = new Random();
            var countDataSets = random.Next(1, 5);

            for (int i = 1; i <= countDataSets; i++)
            {
                var set = CreateDataSet();
                task.DataSets.Add(set);
            }

            return task;
        }

        private DataSet CreateDataSet()
        {
            var inputData = new List<IUnit>();
            var outputData = new List<IUnit>();

            var random = new Random();
            var countInputData = random.Next(1, 5);
            var countOutputData = random.Next(1, 5);

            for (int i = 0; i < countInputData; i++)
            {
                var unit = CreateUnit();
                inputData.Add(unit);
            }

            for (int i = 0; i < countOutputData; i++)
            {
                var unit = CreateUnit();
                outputData.Add(unit);
            }

            return new DataSet(inputData, outputData);
        }

        private IUnit CreateUnit()
        {
            var random = new Random();
            int typeUnit = random.Next(1, 5);

            switch (typeUnit)
            {
                case 1: return new UnitInt(random.Next(-100, 101));
                case 2: return new UnitDouble(random.NextDouble() * (100.0 - -100.0) + 100.0);
                case 3: return new UnitChar((char)random.Next('a', 'z'));
                case 4: return new UnitString(DateTime.Now.Millisecond.ToString());
                default:
                    throw new Exception();
            }
        }
    }
}