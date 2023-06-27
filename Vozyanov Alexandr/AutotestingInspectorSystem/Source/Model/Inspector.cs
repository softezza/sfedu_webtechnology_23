using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using LaboratoryWorkSystem;
using System.IO;

namespace AutotestingInspectorSystem
{
    public class Inspector
    {
        private List<DataLabWorkFile> _historyLabWorks;

        public List<DataLabWorkFile> HistoryLabWorks => _historyLabWorks;

        private Inspector() { }

        public async Task<Editor> OpenLabWork(string path)
        {
            var (labWork, helper) = await LaboratoryWork.FileHelper.OpenFile(path);

            var data = new DataLabWorkFile(labWork.Name, path, DateTime.Now);

            var index = _historyLabWorks.FindIndex(d => d.Path == path);
            if (index < 0) _historyLabWorks.Add(data);
            else _historyLabWorks[index] = data;

            await DataProvider.UnloadHistory(_historyLabWorks);

            return new Editor(labWork, helper);
        }

        public async Task<Editor> CreateLabWork(string name, string directory)
        {
            var labWork = new LaboratoryWork(name);
            var helper = await LaboratoryWork.FileHelper.CreateFile(labWork, directory);

            var data = new DataLabWorkFile(labWork.Name, helper.FileInfo.FullName, DateTime.Now);

            var index = _historyLabWorks.FindIndex(d => d.Path == data.Path);
            if (index < 0) _historyLabWorks.Add(data);
            else _historyLabWorks[index] = data;

            await DataProvider.UnloadHistory(_historyLabWorks);

            return new Editor(labWork, helper);
        }

        public async Task CloseEditor(Editor editor)
        {
            var labWork = editor.LaboratoryWork as LaboratoryWork;
            var path = editor.Path;
            var helper = editor.FileHelper;

            await helper.Save(labWork);

            helper.Close();
        }

        public async static Task<Inspector> CreateInspector()
        {
            DataProvider.Initialize();

            var history = await DataProvider.LoadHistory();

            return new Inspector()
            {
                _historyLabWorks = history
            };
        }
    }

    public struct DataLabWorkFile
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public DateTime DateTime { get; private set; }

        public DataLabWorkFile(string name, string path, DateTime dateTime)
        {
            Name = name;
            Path = path;
            DateTime = dateTime;
        }
    }
}
