using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet
{
    public class GameSession
    {
        public Participant Host { get; set; }
        public Team Team1 { get; set; }
        public string Team1Guess { get; set; }
        public Team Team2 { get; set; }
        public string Team2Guess { get; set; }
        public List<Participant> Spectators { get; set; }

        public GameSession()
        {
            Team1 = new Team(1, "Team 1");
            Team2 = new Team(2, "Team 2");
            Spectators = new List<Participant>();
        }
    }
}
