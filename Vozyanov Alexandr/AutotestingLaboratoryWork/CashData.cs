using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autotesting
{
    public static class CashData
    {
        //public static Inspector Inspector;

        public const string Version = "0.1.0";

        public static string FIO = "Иван Иванов";
        public static string numberZach = "404";
        public static string group = "АИБ-3-037";

        public static List<LaboratoryWorkSystem.IViewLaboratoryWork> labsLW = new List<LaboratoryWorkSystem.IViewLaboratoryWork>();

        public static Dictionary<LaboratoryWorkSystem.IViewLaboratoryWork, Dictionary<LaboratoryWorkSystem.IViewOption, Dictionary<LaboratoryWorkSystem.IViewTask, bool>>> IsCompleteTasks = new Dictionary<LaboratoryWorkSystem.IViewLaboratoryWork, Dictionary<LaboratoryWorkSystem.IViewOption, Dictionary<LaboratoryWorkSystem.IViewTask, bool>>>();

    }
}
