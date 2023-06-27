using System;

namespace ProgramValidation
{
    public class ValidatorException : Exception
    {
        protected ValidatorException(string message) : base(message) { }
    }

    public class EmptyOrNullReferenceExpectedOutputDataException : ValidatorException
    {
        public EmptyOrNullReferenceExpectedOutputDataException(int number)
            : base("The expected output data is empty or NULL. \n" +
                  $"Number Data Set: {number}") { }
    }

    public class NullReferenceExpectedInputDataException : ValidatorException
    {
        public NullReferenceExpectedInputDataException(int number)
            : base("The input data is NULL. \n" +
                  $"Number Data Set: {number}")
        { }
    }

    public class ExeNotFoundException : ValidatorException
    {
        public ExeNotFoundException(string fileExe)
            : base($"Exe File not found: {fileExe}") { }
    }
}
