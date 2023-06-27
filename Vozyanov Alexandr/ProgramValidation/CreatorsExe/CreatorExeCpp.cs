using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using ProgramValidation.Utils;

namespace ProgramValidation.CreatorsExe
{
    class CreatorExeCpp : ICreator
    {
        static string DefaultPathCompiler = @"GCC\mingw64\bin\g++.exe";

        public async Task<string> CreateExeFile(string code)
        {
            var tempFolder = GetTemporaryDirectory();

            var pathGCC = Path.Combine(Environment.CurrentDirectory, DefaultPathCompiler);
            if (!File.Exists(pathGCC)) throw new CompilerNotFoundException(pathGCC);

            var codeFile = Path.Combine(tempFolder, @"Code.cpp");
            var exeFile = Path.Combine(tempFolder, @"Code.exe");

            using (var stream = File.Create(codeFile))
            {
                var bytes = new UTF8Encoding(true).GetBytes(code);
                stream.Write(bytes, 0, bytes.Length);
            }

            var argumentsGCC = $"\"{codeFile}\" -o \"{exeFile}\"";
            var infoGCC = new ProcessStartInfo(pathGCC)
            {
                FileName = pathGCC,
                Arguments = argumentsGCC,
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            var process = Process.Start(infoGCC);
            if (process == null) throw new IncorrectArgumentsException(pathGCC, argumentsGCC);
            await process.WaitForExitAsync();

            if (!File.Exists(exeFile)) return "";

            return exeFile;
        }

        private string GetTemporaryDirectory()
        {
            string TempFolderName = @"AutotesterTemp";
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            tempDirectory += TempFolderName;

            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
