import { Entity } from './Entity';
import { FightTurn } from './FightTurn';
export class Fight {
    turns: FightTurn[];
    player?: Entity;
    enemy?: Entity;

    constructor(turns: FightTurn[], player: Entity, enemy: Entity){
        this.turns = turns;
        this.player = player;
        this.enemy = enemy;
    }
}