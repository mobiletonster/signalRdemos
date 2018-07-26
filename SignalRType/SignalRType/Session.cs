using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRType
{
    public class Session
    {
        public List<Participant> Players { get; set; }

        public Session()
        {
            Players = new List<Participant>();
        }
    }


}
