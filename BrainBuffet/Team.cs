using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Participant> Members { get; set; }
        public bool IsFull
        {
            get
            {
                return Members.Count >= 4;
            }
        }

        public int Available
        {
            get
            {
                return 4 - Members.Count;
            }
        }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
            Members = new List<Participant>();
        }
        public Team()
        {
            Members = new List<Participant>();
        }
        public bool Add(Participant participant)
        {
            if (!IsFull)
            {
                Members.Add(participant);
                return true;
            }
            return false;
        }
    }
}
