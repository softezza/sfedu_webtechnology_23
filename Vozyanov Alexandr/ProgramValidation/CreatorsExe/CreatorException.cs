using System;

namespace ProgramValidation.CreatorsExe
{
    public class CreatorException : Exception
    {
        protected CreatorException(string pathCompiler, string message) 
            : base($"The path to the compiler: {pathCompiler}\n {message}") { }
    }

    public class CompilerNotFoundException : CreatorException
    {
        public CompilerNotFoundException(string pathCompiler) 
            : base(pathCompiler, "The compiler with which the EXE should have been created was not found.") { }
    }

    public class IncorrectArgumentsException : CreatorException
    {
        public IncorrectArgumentsException(string pathCompiler, string arguments)
            : base(pathCompiler, "When trying to create an exe file, the command line was incorrectly formed.\n" +
                  $"Argumets: {arguments}") { }
    }
}
