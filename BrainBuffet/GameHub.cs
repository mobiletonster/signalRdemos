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
        public GameHub(){}

        #region SignalR Events
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            var participant = new Participant(Context.ConnectionId);
            await Clients.Caller.SendAsync("Connected", participant);
            await Clients.Caller.SendAsync("GameState", _gameSession);
        }
    
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            Leave(Context.ConnectionId);
            await Clients.All.SendAsync("GameState", _gameSession);
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region SignalR Client Invokable Endpoints
        // client invokable endpoints
        public async Task Join(string role,Participant participant)
        {
            if(JoinRole(role, participant))
            {
                // if successful, set the user's role and send back.
                participant.Role = role;
            }
            await Clients.All.SendAsync("GameState", _gameSession);
            await Clients.Caller.SendAsync("Joined", participant);
        }

        public async Task QuitRole(Participant participant)
        {
            Leave(participant.ConnectionId);
            await Clients.All.SendAsync("GameState", _gameSession);
            participant.Role = null;
            participant.Name = null;
            await Clients.Caller.SendAsync("Left", participant);
        }
        #endregion

        #region Private Methods
        private bool JoinRole(string role, Participant participant)
        {
            if (role == "host")
            {
                if (_gameSession.Host == null)
                {
                    // participant.Role = role;
                    _gameSession.Host = participant;
                    return true;
                }
                return false;
            }
            if (role == "team1")
            {
                return _gameSession.Team1.Add(participant);
            }
            else if (role == "team2")
            {
                return _gameSession.Team2.Add(participant);
            }
            else if (role == "spectator")
            {
                _gameSession.Spectators.Add(participant);
                return true;
            }
            return false;
        }

        private void Leave(string connectionId)
        {
            if (_gameSession.Host?.ConnectionId == connectionId)
            {
                _gameSession.Host = null;
            }
            _gameSession.Team1.Members.RemoveAll(m => m.ConnectionId == connectionId);
            _gameSession.Team2.Members.RemoveAll(m => m.ConnectionId == connectionId);
            _gameSession.Spectators.RemoveAll(m => m.ConnectionId == connectionId);
        }
        #endregion
    }
}
