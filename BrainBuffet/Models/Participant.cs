using System;

namespace BrainBuffet
{
    public class Participant
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public Participant(string connectionId)
        {
            ConnectionId = connectionId;
        }
        public Participant()
        {

        }
    }
}