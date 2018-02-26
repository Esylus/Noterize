using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePractice.Entities
{
    class Preset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserList { get; set; }

        public Preset(string name, string list)
        {

            this.Name = name;
            this.UserList = list;
        }
    }
}
