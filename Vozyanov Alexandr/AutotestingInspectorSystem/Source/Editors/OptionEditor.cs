using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaboratoryWorkSystem;

namespace AutotestingInspectorSystem
{
    public class OptionEditor
    {
        Option _option;

        public IViewOption Option => _option;
        public IReadOnlyCollection<IViewTask> Tasks => _option.Tasks;

        public OptionEditor(IViewOption option)
        {
            _option = option as Option;
        }

        public IViewTask CreateTask(int number)
        {
            var tasks = _option.Tasks;

            var mathTask = tasks.Find(t => t.Number == number);
            if (mathTask != null) throw new CreatingTaskException(number);

            var task = new Task(number);
            tasks.Add(task);

            return task;
        }

        public void RemoveTask(IViewTask viewTask)
        {
            var tasks = _option.Tasks;
            var task = viewTask as Task;

            if (!tasks.Contains(task)) throw new RemovingTaskException(task);
            tasks.Remove(task);
        }

        public void UpdateNumberTask(IViewTask viewTask, int number)
        {
            var tasks = _option.Tasks;
            var task = viewTask as Task;

            var mathTask = tasks.Find(t => t.Number == number);
            if (mathTask != null) throw new UpdatingNumberTaskException(number);

            task.Number = number;
        }

        public TaskEditor OpenTask(IViewTask task) => new TaskEditor(task);
    }
}
