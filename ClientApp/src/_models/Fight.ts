import { Entity } from './Entity';
import { FightTurn } from './FightTurn';
export class Fight {
    turns: FightTurn[];
    player?: Entity;
    enemy?: Entity;
}