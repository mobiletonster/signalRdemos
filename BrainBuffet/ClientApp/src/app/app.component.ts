import { Component, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { GameSession } from './models/gameSession';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Brain Buffet';
  public _hubConnection: HubConnection | undefined;
  public async: any;
  message: string = '';
  messages: string[]= [];
  startupState: string = "begin";
  loading: boolean = false;
  player: string;

  hasHost: boolean = false;


  ngOnInit() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:8000/api/gamehub')
      .configureLogging(signalR.LogLevel.Information)
      .build();



    this._hubConnection.on('Send', (data: any) => {
      this.checkGameSessionState(data);
      const received = `Received: ${data}`;
      console.log(data);
      // this.messages.push(received);
    })

    this._hubConnection.on('Connected', (data: GameSession) => {
      this.checkGameSessionState(data);
    })
  }

  checkGameSessionState(gameSession: GameSession) {
    if (gameSession.host) {
      console.log("we have a host.");
    }
    if (gameSession.team1.isFull) {
      console.log("team 1 is full");
    }
    if (gameSession.team2.isFull) {
      console.log("team 2 is full");
    }
  }

  public sendMesage(message:string): void {
    const data = `Sent: ${message}`;
    if (this._hubConnection) {
      this._hubConnection.invoke('Send', data);
    }
    this.messages.push(data);
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
}


