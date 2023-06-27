using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaboratoryWorkSystem;
using ProgramValidation;

namespace AutotestingInspectorSystem
{
    public class TaskEditor
    {
        Task _task;
        Task _bufferTask;

        public IViewTask Task => _bufferTask;

        public TaskEditor(IViewTask task)
        {
            _task = task as Task;
            _bufferTask = _task.Clone() as Task;
        }

        public void UpdateDescription(string description)
        {
            _bufferTask.Description = description;
        }

        public void CreateDataSet()
        {
            var sets = _bufferTask.DataSets;
            var set = new DataSet(new List<IUnit>(), new List<IUnit>());

            sets.Add(set);
        }

        public void RemoveDataSet(int index)
        {
            var sets = _bufferTask.DataSets;
            sets.RemoveAt(index);
        }

        public DataSetEditor OpenDataSet(int index)
        {
            return new DataSetEditor(_bufferTask.DataSets[index]);
        }

        public void SaveStatus()
        {
            _task.Update(_bufferTask);
        }
    }
}
