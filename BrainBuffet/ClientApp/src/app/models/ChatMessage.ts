export class ChatMessage {
  public name: string;
  public message: string;
  public isSelf: boolean;

  constructor(name: string, message: string, isSelf:boolean) {
    this.name = name;
    this.message = message;
    this.isSelf = isSelf;
  }
}
