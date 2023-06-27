using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgramValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramValidation.Tests
{
    [TestClass()]
    public class DataSetTests
    {
        [TestMethod()]
        public void CloneTest()
        {
            var set1 = new DataSet(new List<IUnit>() { new UnitInt(32), new UnitDouble(2.0), }, new List<IUnit>() { new UnitChar('k') });
            var set2 = set1.Clone() as DataSet;

            set1.InputData[0] = new UnitInt(12);


            Assert.AreEqual(new UnitInt(32), set2.InputData[0]);
            Assert.AreEqual(new UnitInt(12), set1.InputData[0]);

            set2.ExpectedOutputData[0] = new UnitChar('w');

            Assert.AreEqual(new UnitChar('w'), set2.ExpectedOutputData[0]);
            Assert.AreEqual(new UnitChar('k'), set1.ExpectedOutputData[0]);
        }
    }
}