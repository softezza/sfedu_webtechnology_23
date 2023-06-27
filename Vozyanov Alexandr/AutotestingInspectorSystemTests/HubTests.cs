using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutotestingInspectorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using System.IO;
using ProgramValidation;
using LaboratoryWorkSystem;

namespace AutotestingInspectorSystem.Tests
{
    [TestClass()]
    public class HubTests
    {
        [TestMethod()]
        public async Task LoadHistoryTest()
        {
            var inspector = await Inspector.CreateInspector();
            var history = inspector.HistoryLabWorks;
        }

        [TestMethod()]
        public async Task InspectorLiveCicleTest()
        {
            var pathTestingFolder = InitializeTestingFolder();
            var labNames = InitializeTestLabNames(pathTestingFolder);

            //Создание лабораторной
            var inspector = await Inspector.CreateInspector();
            var history = inspector.HistoryLabWorks;

            var expectedPath = Path.Combine(pathTestingFolder, labNames[0]) + LaboratoryWork.FileHelper.Extension;
            var editor = await inspector.CreateLabWork(labNames[0], pathTestingFolder);
            Assert.AreEqual(editor.LaboratoryWork.Name, labNames[0]);
            Assert.AreEqual(editor.Path, expectedPath);
            await inspector.CloseEditor(editor);


            //Повторное создание лабораторной, проверка на исключения.
            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;

            var ex = await Assert.ThrowsExceptionAsync<FileExistenceException>(async () 
                => await inspector.CreateLabWork(labNames[0], pathTestingFolder));

            Assert.IsTrue(ex.TypeException == FileExistenceException.Type.Overwriting);


            //Открытие лабораторной
            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;

            editor = await inspector.OpenLabWork(expectedPath);
            Assert.AreEqual(editor.LaboratoryWork.Name, labNames[0]);
            Assert.AreEqual(editor.Path, expectedPath);
            await inspector.CloseEditor(editor);


            //Повторное открытие лабораторной
            File.Delete(expectedPath);
            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;

            ex = await Assert.ThrowsExceptionAsync<FileExistenceException>(async ()
                => await inspector.OpenLabWork(expectedPath));

            Assert.IsTrue(ex.TypeException == FileExistenceException.Type.NotExists);


            //Проверка на удаление файла в момент выбора лабораторной
            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;

            editor = await inspector.CreateLabWork(labNames[0], pathTestingFolder);
            await inspector.CloseEditor(editor);

            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;
            File.Delete(expectedPath);

            ex = await Assert.ThrowsExceptionAsync<FileExistenceException>(async () 
                => await inspector.OpenLabWork(expectedPath));
            Assert.IsTrue(ex.TypeException == FileExistenceException.Type.NotExists);

            //Проверка на занятость редакторов файлов.
            inspector = await Inspector.CreateInspector();
            history = inspector.HistoryLabWorks;

            editor = await inspector.CreateLabWork(labNames[0], pathTestingFolder); 

            try
            {
                File.Delete(expectedPath);
                File.Move(expectedPath, Path.Combine(pathTestingFolder, "test") + ".lw");
                Assert.Fail();
            }
            catch (IOException)
            {

            }
            finally
            {
                await inspector.CloseEditor(editor);
            }
        }

        [TestMethod()]
        public async Task LoadUnloadLabWorkTest()
        {
            var pathTestingFolder = InitializeTestingFolder();
            var labNames = InitializeTestLabNames(pathTestingFolder);

            //Создание пустой лабораторной работы.
            var inspector = await Inspector.CreateInspector();

            var expectedPath = Path.Combine(pathTestingFolder, labNames[0]) + LaboratoryWork.FileHelper.Extension;
            var editor = await inspector.CreateLabWork(labNames[0], pathTestingFolder);
            Assert.AreEqual(editor.LaboratoryWork.Name, labNames[0]);
            Assert.AreEqual(editor.Path, expectedPath);
            await inspector.CloseEditor(editor);

            //Открытие пустой лабораторной работы
            inspector = await Inspector.CreateInspector();
            editor = await inspector.OpenLabWork(expectedPath);

            Assert.AreEqual(editor.LaboratoryWork.Name, labNames[0]);
            Assert.AreEqual(editor.LaboratoryWork.Options.Count, 0);

            //Добавление пустового варианта
            editor.CreateOption(1);
            await inspector.CloseEditor(editor);

            //Открытие лабораторной работы с пустым вариантом
            inspector = await Inspector.CreateInspector();
            editor = await inspector.OpenLabWork(expectedPath);

            Assert.AreEqual(editor.LaboratoryWork.Name, labNames[0]);
            Assert.AreEqual(editor.LaboratoryWork.Options.Count, 1);
        }



        private string InitializeTestingFolder()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var pathTestingFolder = currentDirectory + @"\Testing Labs";

            if (!Directory.Exists(pathTestingFolder)) Directory.CreateDirectory(pathTestingFolder);

            return pathTestingFolder;
        }

        private string[] InitializeTestLabNames(string pathTestingFolder)
        {
            var labNames = new string[]
            {
              "Лабораторная работа №1",
              "Лабораторная работа №2",
              "Лабораторная работа №3",
              "Лабораторная работа №4",
              "Лабораторная работа №5",
              "Лабораторная работа №6",
            };

            foreach (var name in labNames)
            {
                var path = Path.Combine(pathTestingFolder, name) + LaboratoryWork.FileHelper.Extension;
                if (File.Exists(path)) File.Delete(path);
            }

            return labNames;
        }

    }
}