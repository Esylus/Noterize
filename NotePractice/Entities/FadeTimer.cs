using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePractice.Entities
{
    class FadeTimer
    {
        public int FadeTimerCount { get; set; }
        public bool PositiveOrNegativePoints { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }


        public FadeTimer(int r, int g, int b)
        {
            FadeTimerCount = 50;
            R = r;
            G = g;
            B = b;
        }

    }
}
