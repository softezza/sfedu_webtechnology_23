using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LaboratoryWorkSystem;
using ProgramValidation;

namespace AutotestingInspectorSystem
{
    public class Editor
    {
        LaboratoryWork _labWork;
        public IViewLaboratoryWork LaboratoryWork => _labWork;
        public IReadOnlyCollection<IViewOption> Options => _labWork.Options;

        public Editor(LaboratoryWork labWork, LaboratoryWork.FileHelper fileHelper)
        {
            _labWork = labWork;
            FileHelper = fileHelper;
            Path = fileHelper.FileInfo.FullName;
        }

        public LaboratoryWork.FileHelper FileHelper { get; private set; }
        public string Path { get; private set; }

        public async System.Threading.Tasks.Task SaveLabWork()
            => await FileHelper.Save(_labWork);


        public IViewOption CreateOption(int number)
        {
            var options = _labWork.Options;
            
            var mathOption = options.Find(o => o.Number == number);
            if (mathOption != null) throw new CreatingOptionException(number);

            var option = new Option(number);
            options.Add(option);

            return option;
        }

        public void RemoveOption(IViewOption viewOption)
        {
            var options = _labWork.Options;
            var option = viewOption as Option;

            if (!options.Contains(option)) throw new RemovingOptionException(option);
            options.Remove(option);
        }

        public void UpdateNumberOption(IViewOption viewOption, int number)
        {
            var options = _labWork.Options;
            var option = viewOption as Option;

            var mathOption = options.Find(o => o.Number == number);
            if (mathOption != null) throw new UpdatingNumberOptionException(number);

            option.Number = number;
        }

        public OptionEditor OpenOption(IViewOption option) => new OptionEditor(option);
    }
}
