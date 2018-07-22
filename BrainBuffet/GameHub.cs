using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet
{
    public class GameHub:Hub
    {
        public static GameSession _gameSession= new GameSession();
        public GameHub()
        {
            // _gameSession = new GameSession();
        }
        public Task Send(string data)
        {
            return Clients.All.SendAsync("Send", data);
        }

        public async Task Join(string role,string player)
        {
            var participant = new Participant(Context.ConnectionId, player);
            switch (role)
            {
                case "host":
                    SetHost(participant);
                    break;
                case "team1":
                case "team2":
                case "spectator":
                    JoinTeam(role, participant);
                    break;
                default:
                    break;
            }
            await Clients.All.SendAsync("Joined", _gameSession);
        }

        public bool SetHost(Participant participant)
        {
            if (_gameSession.Host == null)
            {
                _gameSession.Host = participant;
                return true;
            }
            return false;
        }

        public bool JoinTeam(string team ,Participant participant)
        {
            if (team == "team1")
            {
                return _gameSession.Team1.Add(participant);
            } else if (team == "team2")
            {
                return _gameSession.Team2.Add(participant);
            }
            else if(team=="spectator")
            {
                _gameSession.Spectators.Add(participant);
                return true;
            }
            return false;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("Joined", _gameSession);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            if(Context.ConnectionId == _gameSession?.Host?.ConnectionId)
            {
                _gameSession.Host = null;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await Clients.All.SendAsync("Connected", Context.ConnectionId + " Left");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
