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
            JoinRole(role, participant);
            await Clients.All.SendAsync("Joined", _gameSession);
        }

        public bool JoinRole(string role ,Participant participant)
        {
            if (role == "host")
            {
                if (_gameSession.Host == null)
                {
                    _gameSession.Host = participant;
                    return true;
                }
                return false;
            }
            if (role == "team1")
            {
                return _gameSession.Team1.Add(participant);
            } else if (role == "team2")
            {
                return _gameSession.Team2.Add(participant);
            }
            else if(role=="spectator")
            {
                _gameSession.Spectators.Add(participant);
                return true;
            }
            return false;
        }

        public void Leave(string connectionId)
        {
            if (_gameSession.Host.ConnectionId == connectionId)
            {
                _gameSession.Host = null;
            }
            _gameSession.Team1.Members.RemoveAll(m=>m.ConnectionId==connectionId);
            _gameSession.Team2.Members.RemoveAll(m => m.ConnectionId == connectionId);
            _gameSession.Spectators.RemoveAll(m => m.ConnectionId == connectionId);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("Joined", _gameSession);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            Leave(Context.ConnectionId);
            await Clients.All.SendAsync("Joined", _gameSession);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await Clients.All.SendAsync("Connected", Context.ConnectionId + " Left");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
