using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRType
{
    public class Participant
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; }

        public Participant()
        {
        }

        public Participant(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
            IsReady = false;
        }
    }
}
