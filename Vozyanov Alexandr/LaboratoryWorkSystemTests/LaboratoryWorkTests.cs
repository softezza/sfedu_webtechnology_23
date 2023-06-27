using Microsoft.VisualStudio.TestTools.UnitTesting;
using LaboratoryWorkSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramValidation;

namespace LaboratoryWorkSystem.Tests
{
    [TestClass()]
    public class LaboratoryWorkTests
    {
        [TestMethod()]
        public void LaboratoryWorkTest()
        {
            //var lw = new LaboratoryWork("Лабораторная работа №1");

            //AssertInitOptions(lw, 5);

            //var random = new Random();
            //var selectedNumber = random.Next(1, lw.Options.Count());

            //var selectedOption = lw.Options.FirstOrDefault(option => option.Number == selectedNumber);

            //AssertInitTasks(selectedOption as Option, 5);
            //}
        }

        private void AssertInitOptions(LaboratoryWork lw, int countOptions)
        {
            //#region Init
            //var expectedOptions = new IViewOption[countOptions];

            //for (int i = 0; i < countOptions; i++)
            //{
            //    lw.CreateOption(i + 1);
            //    expectedOptions[i] = new Option(i + 1);
            //}

            //var options = lw.Options;
            //var checkOptions = options.ToList();

            //Assert.AreEqual(checkOptions.Count, expectedOptions.Length);

            //for (int i = 0; i < expectedOptions.Length; i++)
            //    Assert.AreEqual(checkOptions[i].Number, expectedOptions[i].Number);
            //#endregion

            //var random = new Random();
            //IViewOption GetOptionOfNumber(int number) => options.FirstOrDefault(o => o.Number == number);

            //#region Remove
            //var removedNumber = random.Next(1, countOptions + 1);

            //lw.RemoveOption(GetOptionOfNumber(removedNumber));
            //Assert.IsNull(GetOptionOfNumber(removedNumber));

            //lw.CreateOption(removedNumber);

            //#endregion

            //#region Update
            //var updatedNumber = random.Next(1, countOptions + 1);
            //var newNumber = countOptions + 1;

            //lw.UpdateNumberOption(GetOptionOfNumber(updatedNumber), newNumber);

            //var checkRenamedOption = GetOptionOfNumber(newNumber);
            //Assert.IsNotNull(checkRenamedOption);
            //Assert.AreEqual(checkRenamedOption.Number, newNumber);

            //lw.UpdateNumberOption(GetOptionOfNumber(newNumber), updatedNumber);
            //#endregion
        }
        private void AssertInitTasks(Option option, int countTasks)
        {
            //#region Init

            //var expectedTasks = new IViewTask[countTasks];

            //for (int i = 0; i < countTasks; i++)
            //{
            //    option.CreateTask(i + 1);
            //    expectedTasks[i] = new Task(i + 1);
            //}

            //var tasks = option.Tasks;
            //var checkTasks = tasks.ToList();

            //Assert.AreEqual(checkTasks.Count, expectedTasks.Length);

            //for (int i = 0; i < expectedTasks.Length; i++)
            //    Assert.AreEqual(checkTasks[i].Number, checkTasks[i].Number);

            //#endregion

            //var random = new Random();
            //IViewTask GetTaskOfNumber(int number) => tasks.FirstOrDefault(t => t.Number == number);

            //#region Remove
            //var removedNumber = random.Next(1, countTasks + 1);

            //option.RemoveTask(GetTaskOfNumber(removedNumber));
            //Assert.IsNull(GetTaskOfNumber(removedNumber));

            //option.CreateTask(removedNumber);
            //#endregion

            //#region Update
            //var updatedNumber = random.Next(1, countTasks + 1);
            //var newNumber = countTasks + 1;

            //option.UpdateNumberTask(GetTaskOfNumber(updatedNumber), newNumber);
            //var checkRenamedOption = GetTaskOfNumber(newNumber);
            //Assert.IsNotNull(checkRenamedOption);
            //Assert.AreEqual(checkRenamedOption.Number, newNumber);

            //option.UpdateNumberTask(GetTaskOfNumber(newNumber), updatedNumber);
            //#endregion

          }

    }
}