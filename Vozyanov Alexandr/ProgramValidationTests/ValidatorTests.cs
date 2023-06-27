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
    public class ValidatorTests
    {
        static string code =
            @"#include <iostream>
            using namespace std;
            int main()
            {
              double a; double b;
              cin >> a; cin >> b;
              cout << a + b;
            }";

        static DataSet[] Datas = new DataSet[]
        {
            new DataSet(new List<IUnit>() { new UnitDouble(5.4), new UnitDouble(2.3) }, new List<IUnit>() { new UnitDouble(7.7) }),
            new DataSet(new List<IUnit>() { new UnitDouble(1.15), new UnitDouble(2.23) }, new List<IUnit>() { new UnitDouble(3.38) }),
            new DataSet(new List<IUnit>() { new UnitInt(2), new UnitInt(2) }, new List<IUnit>() { new UnitInt(4) }),
        };

        [TestMethod()]
        public void ValidateCodeCppTest()
        {
            ValidateCodeCppAsync();
        }

        async void ValidateCodeCppAsync()
        {
            var validator = new Validator();
            var result = await validator.ValidateCodeCpp(code, Datas);

            Assert.IsTrue(result);
        }
    }
}