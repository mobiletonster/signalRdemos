import { Participant } from "./participant";
import { Team } from "./Team";

export class GameSession {
  public host: Participant;
  public team1: Team;
  public team2: Team;
  public spectators: Participant[];
}
