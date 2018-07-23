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
  public _hubConnection: HubConnection | undefined;
  _startupState: string = "begin";
  _participant: Participant;
  _gameSession: GameSession = new GameSession();
  _loading: boolean = false;
  _playerName: string;

  ngOnInit() {

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/api/gamehub')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.on('Connected', (participant: Participant) => {
      // on connected, get initial state of participant (with connection id only)
      this._participant = participant;
    });

    this._hubConnection.on('Joined', (participant: Participant) => {
      // on join, get updated participant with assigned role.
      this._participant = participant;
      if (participant.role == "team1" || participant.role == "team2") {
        this._startupState = "team";
      } else {
        this._startupState = participant.role;
      }
    });

    this._hubConnection.on('Left', (participant: Participant) => {
      // on leave, reset the user back to a state where they can rejoin.
      this._participant = participant;
      this._startupState="chooser"
    })

    this._hubConnection.on('GameState', (gameSession: GameSession) => {
      this._gameSession = gameSession;
    });
  }

  // enter key pressed in name field
  keyDown(event) {
    if (event.keyCode == 13) {
      this.start_click();
    }
  }

  // Click Events
  public start_click() {
    if (this._playerName == undefined || this._playerName.length < 2) {
      alert('Please enter your name!')
      return;
    }
    this._loading = true;

    this._hubConnection.start()
      .then(() => {
        setTimeout(() => {
          this._startupState = "chooser";
          this._loading = false;
        }, 200);     // wait a while so we can see the cool loading animation.
    }).catch(err => {
      console.error(err.toString());
      this._loading = false;
    });
  }

  public join_click(role: string): void {
    if (this._hubConnection) {
      this._participant.name = this._playerName;
      this._hubConnection.invoke('Join', role, this._participant);
    }
  }

  public quitRole_click() {
    if (this._hubConnection) {
      this._hubConnection.invoke('QuitRole', this._participant);
    }
  }

  // Class Togglers
  isDisabledBtn(state: boolean) {
    if (state) {
      return 'btn-disabled';
    } else {
      return 'btn';
    }
  }

  roleClass(state: string) {
    switch (state) {
      case 'host':
        return "fas fa-microphone-alt";
      case 'team1':
        return "fab fa-rebel";
      case 'team2':
        return "fab fa-empire";
      case 'spectator':
        return "fas fa-users";
    }
  }
}


