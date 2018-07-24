import { Participant } from "./participant";
import { Team } from "./Team";

export class GameSession {
  public host: Participant;
  public team1: Team;
  public team1Guess: string;
  public team2: Team;
  public team2Guess: string;
  public spectators: Participant[];
}
