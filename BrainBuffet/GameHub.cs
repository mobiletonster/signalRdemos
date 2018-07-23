using BrainBuffet.Models;
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
        public GameHub(){
        }

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
                // add them to their group so they can chat privately within that group
                // host is a lonely group of one.
                await Groups.AddToGroupAsync(participant.ConnectionId, role);
            }
            await Clients.All.SendAsync("GameState", _gameSession);
            await Clients.Caller.SendAsync("Joined", participant);
        }
        public async Task QuitRole(Participant participant)
        {
            await Groups.RemoveFromGroupAsync(participant.ConnectionId, participant.Role);
            Leave(participant.ConnectionId);
            await Clients.All.SendAsync("GameState", _gameSession);
            participant.Role = null;
            participant.Name = null;
            await Clients.Caller.SendAsync("Left", participant);
        }
        public async Task TeamChat(Participant participant, string message)
        {
            await Clients.OthersInGroup(participant.Role).SendAsync("TeamMessage",participant.Role,participant.Name, message);
            if (participant.Role != "spectator")
            {
                // let spectators listen in on team1 and team2 chats.
                await Clients.Group("spectator").SendAsync("TeamMessage", participant.Role, participant.Name, message);
            }
        }
        public async Task PushQuestion(Question question)
        { 
            await Clients.Groups("spectator","team1","team2").SendAsync("LoadQuestion", question);
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

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
        #endregion
    }
}
