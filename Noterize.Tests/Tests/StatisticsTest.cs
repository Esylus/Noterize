using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotePractice.Entities;
using NUnit.Framework;

namespace Noterize.Tests.Tests
{
    [TestFixture]
    class StatisticsTest
    {
        [Test]
        public void calculatorScore_PositiveDecimals_ReturnPositiveDecimal()
        {   // Test score calculator
            //Arrange, Act, Assert 

            decimal total = 10.0m;
            decimal correct = 4.0m;
            decimal expected = 0.4m;
            decimal actual;

            Statistics testStatistics = new Statistics();

            actual = testStatistics.CalculateAccuracy(correct, total);

            Assert.AreEqual(expected, actual);
        }
    }
}
