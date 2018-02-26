using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePractice.Entities
{
    class GameTimer
    {

        public int TimerCount { get; set; }

        public GameTimer()
        {
            TimerCount = 60;      // set how long each round should take 
        }

    }
}
