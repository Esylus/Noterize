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
    class KeyRandomizerTest
    {
        [Test]
        public void getRandomIntFromAnyList_IntegerList_ReturnPositiveInteger()
        {   // test Randomizer functionality, input numbers and return number in same range 
            // Arrange, Act, Assert

            List<int> testList = new List<int>();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                testList.Add(random.Next(0, 100));
            }

            int actual;

            KeyRandomizer testRandomizer = new KeyRandomizer();

            actual = testRandomizer.GetRandomIntFromAnyList(testList);

            if ((actual >= 0) && (actual <= testList.Count()))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void preventDuplicates_IntegerList_ReturnUniqueNumberComparedToPrevious()
        {// Prevent Duplicates should deliver multiple integers without ever repeating two
            // Arrange Act Assert

            KeyRandomizer testPreventDuplicates = new KeyRandomizer();
            List<int> testList = new List<int>();
            Random random = new Random();
            int previousNumber = -1;
            int currentNumber = 0;

            for (int i = 0; i < 100; i++)
            {
                testList.Add(random.Next(0, 100));
            }

            for (int j = 0; j < 100; j++)
            {
                currentNumber = testPreventDuplicates.PreventDuplicates(testList);

                if (currentNumber == previousNumber)
                {
                    Assert.Fail();
                }
                else
                {
                    previousNumber = currentNumber;
                    Console.WriteLine(currentNumber);
                }

            }

            Assert.Pass();

        }


    }
}
