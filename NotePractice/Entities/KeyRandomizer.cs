using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePractice.Entities
{
    // this class contains all functionality related to extracting a unique (non-duplicated) random value from a user created list 

    public class KeyRandomizer
    {
        private List<int> userSelectedKeyList = new List<int>();
        private int LastRandomKey = -1;                          // to track the last key served to prevent duplicates

        public List<int> UserSelectedKeyList { get { return userSelectedKeyList; } }
        public int CurrentRandomKey { get; set; }                 // this is target key the user tries to press       


        public KeyRandomizer()
        {
        }

        public KeyRandomizer(List<int> populateUserSelectedKeyList)
        {
            userSelectedKeyList.Clear();
            userSelectedKeyList = populateUserSelectedKeyList;
        }

        public void extractUserRandomKeyToMember(List<int> userList)
        {//for any list, get a non-duplicate number and extract it's element to be the CurrentRandomKey

            int nonDuplicateInt = preventDuplicates(userList);
            int selectedKeyFromUserList = userList.ElementAt(nonDuplicateInt);
            CurrentRandomKey = selectedKeyFromUserList;
        }

        public int preventDuplicates(List<int> list)
        {// for any list, select a key that is unique from the key that preceded it

            int randomIntFromUserList = 0;
            int elementAtRandomIntFromUserList = 0;

            do
            {
                randomIntFromUserList = getRandomIntFromAnyList(list);
                elementAtRandomIntFromUserList = list.ElementAt(randomIntFromUserList);

            } while (elementAtRandomIntFromUserList == LastRandomKey);

            LastRandomKey = elementAtRandomIntFromUserList;

            return randomIntFromUserList;
        }

        public int getRandomIntFromAnyList(List<int> rawList)
        {//take any list and get one random value from it

            Random random = new Random();
            int randomIntFromAnyList = random.Next(0, rawList.Count);

            return randomIntFromAnyList;
        }
    }
}
