using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaboratoryWorkSystem;

namespace AutotestingInspectorSystem
{
    public class EditingException : Exception
    {
        public EditingException(string message) : base(message) { }
    }

    public class CreatingOptionException : EditingException
    {
        public int Number { get; private set; }

        public CreatingOptionException(int number) 
            : base($"Вариант с номером {number} уже существует.")
        {
            Number = number;
        }
    }

    public class RemovingOptionException : EditingException
    {
        public IViewOption Option { get; private set; }

        public RemovingOptionException(IViewOption option)
            : base($"Вариант с номером {option.Number} нельзя удалить, так как не находится в списке вариантов.")
        {
            Option = option;
        }
    }

    public class UpdatingNumberOptionException : EditingException
    {
        public int Number { get; private set; }

        public UpdatingNumberOptionException(int number)
            : base($"Невозможно переименовать на номер {number}, так как такой номер уже существует.")
        {
            Number = number;
        }
    }


    public class CreatingTaskException : EditingException
    {
        public int Number { get; private set; }

        public CreatingTaskException(int number)
            : base($"Задача с номером {number} уже существует.")
        {
            Number = number;
        }
    }

    public class RemovingTaskException : EditingException
    {
        public IViewTask Task { get; private set; }

        public RemovingTaskException(IViewTask task)
            : base($"Задача с номером {task.Number} нельзя удалить, так как она не находится в списке задач.")
        {
            Task = task;
        }
    }

    public class UpdatingNumberTaskException : EditingException
    {
        public int Number { get; private set; }

        public UpdatingNumberTaskException(int number)
            : base($"Невозможно переименовать на номер {number}, так как такой номер уже существует.")
        {
            Number = number;
        }
    }

}
