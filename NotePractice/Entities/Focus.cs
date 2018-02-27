using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePractice.Entities
{
    /* Focus class will track accuracy of users performance per keystroke, then break down numbers per unique key for number of attempts and number right.
         From this a performance percentage is calculated.. ex. User got key "B" right 80% of time, key "X" right only 20% of time.
         These percentages are used to calculate the composition of the users next test, if they choose to continue practicing.
         A scale is used to determine what percentage grade will dictate occurences in next list. 
         Lists are cumulative - keys a player is consistently weak at will occur more often so a player has more opportunities to strengthen that key strike.
      ** Interesting note - due to algorithm that prevents keys repeating (two "A"s in a row), no matter the list composition, a letter can only appear max 50% of time.
      ** Note that totalsFocusKeyDistribution, totlsInFocusList are for watching results and thus testing purposes only. 
         */

    public class Focus : KeyRandomizer
    {
        private List<KeyValuePair<int, int>> userResultsList = new List<KeyValuePair<int, int>>(); // Stores results of users test performance per key (key, rightOrWrong) 

        private Dictionary<int, decimal> totalsKeyValuePairList = new Dictionary<int, decimal>();  // Stores processed results as (uniquekey, %correct)

        private List<int> focusList = new List<int>();  // Stores list for users next round, composition based on past performance

        public List<int> FocusList { get { return focusList; } }

        public bool FocusModeEnabled { get; set; }  // will override normal gameplay by using focusedList rather then userSelectedList

        public Focus()
        {
            userResultsList.Clear();
            totalsKeyValuePairList.Clear();
            totalsFocusKeyDistribution.Clear(); // TESTING PURPOSES
            focusList.Clear();
            FocusModeEnabled = false;
        }

        public void RecordUserResults(int currentKey, int rightOrWrong)
        {
            userResultsList.Add(new KeyValuePair<int, int>(currentKey, rightOrWrong));
        }

        //-----------------------------------------Processing----------------------

        public void CreateFocusList()
        {// get all unique keys from users results, parse out attempts and correct strikes to create average

            totalsKeyValuePairList.Clear();
            totalsFocusKeyDistribution.Clear();  // TESTING PURPOSES
            totalsInFocusList.Clear(); // TESTING PURPOSES

            List<int> allKeys = (from kvp in userResultsList select kvp.Key).Distinct().ToList();

            foreach (int uniqueKey in allKeys)
            {
                decimal keyAttempts = (from kvp in userResultsList where kvp.Key == uniqueKey select kvp.Value).Count();

                decimal keySum = (from kvp in userResultsList where kvp.Key == uniqueKey select kvp.Value).Sum();

                decimal average = keySum / keyAttempts;

                totalsFocusKeyDistribution.Add(uniqueKey, keySum);  // TESTING PURPOSES - shows distribution of randomizer per round

                totalsKeyValuePairList.Add(uniqueKey, average);  // store to dictionary
            }

            PopulateFocusList();                               // create new focusList
            userResultsList.Clear();                           // clear results list to prepare for next round

            PopulateTotalInFocusList();  // TESTING PURPOSES ONLY

        }

        public void PopulateFocusList()        // Scale to determine key composition of future test based on past performance
        {
            foreach (var totalPair in totalsKeyValuePairList)
            {
                if (totalPair.Value >= (decimal)0.95)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        focusList.Add(totalPair.Key);
                    }
                }
                else if (totalPair.Value >= (decimal)0.8)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        focusList.Add(totalPair.Key);
                    }
                }
                else if (totalPair.Value >= (decimal)0.5)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        focusList.Add(totalPair.Key);
                    }
                }
                else if (totalPair.Value <= (decimal)0.5)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        focusList.Add(totalPair.Key);
                    }
                }
            }
        }

        //----------------------------START OF TESTING TO BE REMOVED AT END OF PROJECT-----------------------------------------------------------------------------

        private Dictionary<int, decimal> totalsFocusKeyDistribution = new Dictionary<int, decimal>(); // TESTING to observe distrubution of randomly serverd numbers (key, # of occurences)

        private Dictionary<int, int> totalsInFocusList = new Dictionary<int, int>();  // TESTING to observe composition of focus List (key, #ofOccurences)

        private void PopulateTotalInFocusList()   // TESTING PURPOSES ONLY - Create dictionary of focusList composition of occurences
        {
            var g = focusList.GroupBy(i => i);

            foreach (var grp in g)
            {
                totalsInFocusList.Add(grp.Key, grp.Count());
            }
        }

        //---------------------------- END OF TESTING -------------------------------------------------------------------------------------------------------------


    }
}
