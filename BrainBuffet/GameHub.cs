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
        public static GameSession _gameSession;
        public GameHub()
        {
            _gameSession = new GameSession();
        }
        public Task Send(string data)
        {
            return Clients.All.SendAsync("Send", data);
        }

        public async Task Join(string role,string player)
        {
            switch (role)
            {
                case "host":
                    SetHost(new Participant(Context.ConnectionId, player));
                    break;
                case "team1":
                    break;
                case "team2":
                    break;
                default:
                    break;
            }
            await Clients.All.SendAsync("Send", _gameSession);
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

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("Send", _gameSession);
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
