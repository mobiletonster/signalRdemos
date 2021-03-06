﻿using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRType
{
    public class TypeHub : Hub
    {
        public static Session _session = new Session();

        #region SignalR Events
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("LoadPlayers", _session.Players);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _session.Players.RemoveAll(p => p.ConnectionId == Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

        #region User Events
        public async Task SendNewPlayer(string user)
        {
            _session.Players.Add(new Participant(Context.ConnectionId, user));
            await Clients.Others.SendAsync("ReceiveNewPlayer", user, Context.ConnectionId, false);
            await Clients.Caller.SendAsync("ReceiveNewPlayer", user, Context.ConnectionId, true);
        }

        public async Task SendProgress(int progress, string text)
        {
            Participant player = _session.Players.Find(p => p.ConnectionId == Context.ConnectionId);
            await Clients.Others.SendAsync("ReceiveProgress", player, progress, text, false);
            await Clients.Caller.SendAsync("ReceiveProgress", player, progress, text, true);
        }

        public async Task SendStartStatus()
        {
            _session.Players.Find(p => p.ConnectionId == Context.ConnectionId).IsReady = true;

            int numInGame = _session.Players.Count;
            int numReady = _session.Players.FindAll(p => p.IsReady == true).Count;

            await Clients.All.SendAsync("UpdateStartStatus", numInGame, numReady);
        }

        public async Task SendReset()
        {
            foreach (var player in _session.Players)
            {
                player.IsReady = false;
            }

            int numInGame = _session.Players.Count;
            int numReady = _session.Players.FindAll(p => p.IsReady == true).Count;

            await Clients.All.SendAsync("ReceiveNext", numInGame, numReady);
        }


        public async Task SendPlayerReset()
        {
            _session.Players = new List<Participant>();
            await Clients.All.SendAsync("PlayerReset");
        }
        #endregion
    }
}
