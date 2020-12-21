import { Entity } from './Entity';
import { FightTurn } from './FightTurn';
export class Fight {
    turns: FightTurn[];
    playerHealth?: Entity;
    enemyHealth?: Entity;
}