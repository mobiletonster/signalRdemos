using System;

namespace BrainBuffet
{
    public class Participant
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }

        public Participant(string connectionId, string name)
        {
            ConnectionId = connectionId;
            Name = name;
        }
        public Participant()
        {

        }
    }
}