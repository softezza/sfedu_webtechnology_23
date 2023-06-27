using System.Threading.Tasks;

namespace ProgramValidation.CreatorsExe
{
    interface ICreator
    {
        Task<string> CreateExeFile(string code);
    }
}
