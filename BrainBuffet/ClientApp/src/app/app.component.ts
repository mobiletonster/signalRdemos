import { Component, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { GameSession } from './models/gameSession';
import { Participant } from './models/participant';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Brain Buffet';
  public _hubConnection: HubConnection | undefined;s
  _gameSession: GameSession = new GameSession();
  startupState: string = "begin";
  loading: boolean = false;
  player: string;

  hasHost: boolean = false;


  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:8000/api/gamehub')
      .configureLogging(signalR.LogLevel.Information)
      .build();



    this._hubConnection.on('Joined', (gameSession: GameSession) => {
      // on join, get the game session state.
      this._gameSession = gameSession;
    })

    //this._hubConnection.on('Connected', (gameSession: GameSession) => {
    //  //this.checkGameSessionState(gameSession);
    //  console.log(gameSession);
    //  this._gameSession = gameSession;
    //})
  }

  checkGameSessionState(gameSession: GameSession) {
    if (gameSession.host) {
      this._gameSession = gameSession;
      console.log("we have a host.");
    }
    if (gameSession.team1.isFull) {
      console.log("team 1 is full");
    }
    if (gameSession.team2.isFull) {
      console.log("team 2 is full");
    }
  }



  public join(role: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('Join', role, this.player);
    }
  }

  keyDown(event) {
    if (event.keyCode == 13) {
      this.start();
    }
  }

  start() {
    if (this.player==undefined || this.player.length < 2) {
      alert('Please enter your name!')
      return;
    }
    this.loading = true;

    // wait a while so we can see the cool loading animation.

      this._hubConnection.start()
        .then(() => {
          setTimeout(() => {
            this.startupState = "chooser";
            this.loading = false;
          }, 2000);
      }).catch(err => {
        console.error(err.toString());
        this.loading = false;
      });
  }

  choose(role: string) {
    this.join(role);
  }

  isDisabledBtn(state: boolean) {
    if (state) {
      return 'btn-disabled';
    } else {
      return 'btn';
    }
  }
}


