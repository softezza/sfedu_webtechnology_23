using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LaboratoryWork = LaboratoryWorkSystem.LaboratoryWork;

namespace AutotestingInspectorSystem
{
    static class DataProvider
    {
        static string NameFolderAppData => @"Autotesting Inspector";
        static string NameFileHistoryLabWork => @"History Laboratory Works.json";

        static DirectoryInfo _directoryAppData;
        static FileInfo _historyLabWorksFile;

        public static void Initialize()
        {
            _directoryAppData = new DirectoryInfo(Path.Combine(Environment
                .GetFolderPath(Environment.SpecialFolder.ApplicationData), NameFolderAppData));

            _historyLabWorksFile = new FileInfo(Path.Combine(_directoryAppData.FullName, NameFileHistoryLabWork));

            if (!_directoryAppData.Exists) Directory.CreateDirectory(_directoryAppData.FullName);
        }

        public static async Task<List<DataLabWorkFile>> LoadHistory()
        {
            var path = _historyLabWorksFile.FullName;
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    var json = await reader.ReadToEndAsync();
                    var history = await Task.Run(() => JsonConvert.DeserializeObject<List<DataLabWorkFile>>(json));
                    if (history == null) history = new List<DataLabWorkFile>();

                    history = history.FindAll(FilterDataLabWorkFile);
                    history.Sort((data1, data2) => DateTime.Compare(data2.DateTime, data1.DateTime));

                    return history;
                }
            }
            else return new List<DataLabWorkFile>();
        }

        public static async Task UnloadHistory(List<DataLabWorkFile> history)
        {
            var path = _historyLabWorksFile.FullName;
            using (var writer = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write)))
            {
                var json = await Task.Run(() => JsonConvert.SerializeObject(history, Formatting.Indented));
                await writer.WriteAsync(json);
            }
        }

        private static bool FilterDataLabWorkFile(DataLabWorkFile data)
        {
            var fileInfo = new FileInfo(data.Path);

            if (!fileInfo.Exists) return false;
            if (fileInfo.Extension != LaboratoryWork.FileHelper.Extension) return false;

            return true;
        }
    }
}
