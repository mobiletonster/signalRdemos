import { Component, OnInit } from '@angular/core';
import { FormsModule, NgModel } from '@angular/forms';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { GameSession } from './models/gameSession';
import { Participant } from './models/participant';
import { ChatMessage } from './models/ChatMessage';
import { QuestionService } from './services/question.service';
import { Question } from './models/Question';

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
  _chat: string;
  _team1Messages: ChatMessage[] = new Array<ChatMessage>();
  _team2Messages: ChatMessage[] = new Array<ChatMessage>();
  _spectatorMessages: ChatMessage[] = new Array<ChatMessage>();
  _question: Question;
  _guess: string;
  _guessed: boolean;

  constructor(private _questionService: QuestionService) { }

  ngOnInit() {

    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/api/gamehub')
      .configureLogging(signalR.LogLevel.Debug)
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
      this._startupState = "chooser"
      this._question = null;
      this._spectatorMessages = new Array<ChatMessage>();
      this._team1Messages = new Array<ChatMessage>();
      this._team2Messages = new Array<ChatMessage>();
    })

    this._hubConnection.on('GameState', (gameSession: GameSession) => {
      this._gameSession = gameSession;
    });

    this._hubConnection.on('TeamMessage', (team:string, name: string, message: string) => {
      this.addChatMessage(team, new ChatMessage(name, message, false));
    })

    this._hubConnection.on('LoadQuestion', (question: Question) => {
      this._question = question;
      this._guess = null;
      this._guessed = false;
    })

    this._hubConnection.on('GuessSent', (team: string, guess: string) => {
      if (team == "team1") {
        this._gameSession.team1Guess = guess;
      } else if (team == "team2") {
        this._gameSession.team2Guess = guess;
      }
    })

    this._hubConnection.on('AnswerRevealed', (question: Question) => {
      this._question.answerText = question.answerText;
      this._question.reveal = true;
    })

  }

  // enter key pressed in name field
  keyDown(event) {
    if (event.keyCode == 13) {
      this.start_click();
    }
  }

  messageKeyDown(event) {
    if (event.keyCode == 13) {
      this.sendMessage_click();
    }
  }

  guessKeyDown(event) {
    if (event.keyCode == 13) {
      this.sendGuess_click();
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

  public sendMessage_click() {
    if (this._chat.length < 1) { return;}
    if (this._hubConnection) {
      this.addChatMessage(this._participant.role,new ChatMessage(this._participant.name, this._chat, true));
      this._hubConnection.invoke('TeamChat', this._participant, this._chat);
      this._chat = "";
    }
  }

  public getRandomQuestion_click() {
    this._question = null;
    this._questionService.getRandomQuestion().subscribe(question => {
      this._question = question;
      this._gameSession.team1Guess = null;
      this._gameSession.team2Guess = null;
    })
  }

  public push_click(question: Question) {
    let pushQuestion = new Question();
    pushQuestion.id = question.id;
    pushQuestion.questionType = question.questionType;
    pushQuestion.questionText = question.questionText;
    pushQuestion.imageUrl = question.imageUrl;
    if (this._hubConnection) {
      this._hubConnection.invoke('PushQuestion', pushQuestion);
      this._question.pushed = true;
    }
  }

  public sendGuess_click() {
    if (this._hubConnection) {
      this._hubConnection.invoke('GuessAnswer', this._participant, this._guess);
      this._guessed = true;
    }
  }

  public reveal_click() {
    if (this._hubConnection) {
      this._hubConnection.invoke('RevealAnswer', this._question);
      this._question.reveal = true;
    }
  }

  public score_click(team: string, amount:number) {
    if (this._hubConnection) {
      this._hubConnection.invoke('AddScore', team, amount);
    }
  }

  public resetScores_click() {
    if (this._hubConnection) {
      this._hubConnection.invoke('ResetScores');
      document.getElementById("menu-items").style.display = "none";
    }
  }

  // ngClass Togglers
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

  chatBubble(isSelf: boolean) {
    if (isSelf) {
      return 'speech-bubble-self';
    } else {
      return 'speech-bubble-other';
    }
  }

  // private methods
  addChatMessage(team: string, chat: ChatMessage) {
    if (team == "team1") {
      this._team1Messages.push(chat);
      while (this._team1Messages.length > 20) {
        this._team1Messages.shift();
      }
    } else if (team == "team2") {
      this._team2Messages.push(chat);
      while (this._team2Messages.length > 20) {
        this._team2Messages.shift();
      }
    } else if (team == "spectator") {
      this._spectatorMessages.push(chat);
      while (this._spectatorMessages.length > 20) {
        this._team2Messages.shift();
      }
    }
  }

  isCaptain() {
    if (this._participant.role == "team1") {
      return (this._gameSession.team1.members[0].connectionId == this._participant.connectionId);
    } else if (this._participant.role == "team2") {
      return (this._gameSession.team2.members[0].connectionId == this._participant.connectionId);
    }
    return false;
  }
}


