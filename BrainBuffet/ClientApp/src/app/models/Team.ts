import { Participant } from "./participant";

export class Team {
  public id: number;
  public name: string;
  public members: Participant[];
  public get isFull(): boolean {
    return this.members.length >= 4;
  }
}
