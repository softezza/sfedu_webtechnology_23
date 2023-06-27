using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryWorkSystem
{
    public class SerializationException : Exception
    {
        public SerializationException(string message) : base(message)
        {
        }
    }

    public class FileExistenceException : SerializationException
    {
        static Dictionary<Type, string> _typeMessages = new Dictionary<Type, string>()
        {
            [Type.Overwriting] = "Попытка перезаписать существующий файл",
            [Type.NotExists] = "Заданный файл не существует",
        };

        public Type TypeException { get; private set; }

        public FileExistenceException(string fileName, Type type)
            : base($"{_typeMessages[type]}: {fileName}.")
        {
            TypeException = type;
        }

        public enum Type
        {
            Overwriting,
            NotExists,
        }
    }

    public class ExtensionException : SerializationException
    {
        public ExtensionException(string errorExtension, params string[] extensions)
            : base(string
                  .Concat($"Переданный сериализуемый файл не относится к поддерживаемым исключениям: {extensions}.",
                "\n", $"Переданное расширение: {errorExtension}."))
        {

        }
    }

    public class CorruptedFileException : SerializationException
    {
        public CorruptedFileException(string message)
            : base($"Переданный для сериализации файл поврежден.\n {message}")
        {

        }
    }

    public class FormattingException : SerializationException
    {
        public FormattingException(string message) : base(message) { }
    }
}
