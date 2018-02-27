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
    class FocusTest
    {// This test enters test results, processes them in the Focus class then compares the results (in a dictionary) to the prebuilt answer dictionary 

        [Test]
        public void sortList_intoNewList_returnDictionary()
        {//Arrange Act Assert

            Dictionary<int, int> answerDictionary = new Dictionary<int, int>();
            answerDictionary.Add(4, 5);
            answerDictionary.Add(5, 10);
            answerDictionary.Add(6, 15);
            answerDictionary.Add(7, 20);


            Dictionary<int, int> testDictionary = new Dictionary<int, int>();

            Focus testFocus = new Focus();

            testFocus.RecordUserResults(4, 1);
            testFocus.RecordUserResults(5, 1);
            testFocus.RecordUserResults(6, 1);
            testFocus.RecordUserResults(7, 1);

            testFocus.RecordUserResults(4, 1);
            testFocus.RecordUserResults(5, 1);
            testFocus.RecordUserResults(6, 1);
            testFocus.RecordUserResults(7, 1);

            testFocus.RecordUserResults(4, 1);
            testFocus.RecordUserResults(5, 1);
            testFocus.RecordUserResults(6, 1);
            testFocus.RecordUserResults(7, 0);

            testFocus.RecordUserResults(4, 1);
            testFocus.RecordUserResults(5, 1);
            testFocus.RecordUserResults(6, 0);
            testFocus.RecordUserResults(7, 0);

            testFocus.RecordUserResults(4, 1);
            testFocus.RecordUserResults(5, 0);
            testFocus.RecordUserResults(6, 0);
            testFocus.RecordUserResults(7, 0);

            testFocus.CreateFocusList();

            var g = testFocus.FocusList.GroupBy(i => i);

            foreach (var grp in g)
            {
                testDictionary.Add(grp.Key, grp.Count());
            }

            if (testDictionary.SequenceEqual(answerDictionary))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
