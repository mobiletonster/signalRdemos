import { Participant } from "./participant";

export class Team {
  public id: number;
  public name: string;
  public members: Participant[];
  public isFull: boolean;
  public available: number;
}
