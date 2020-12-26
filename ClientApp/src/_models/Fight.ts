import { Entity } from './Entity';
import { FightTurn } from './FightTurn';
export class Fight {
    turns: FightTurn[];
    player?: Entity;
    enemy?: Entity;

    constructor(turns: FightTurn[], playerHealth: number, enemyHealth: number){
        this.turns = turns;
        this.player = new Entity(playerHealth);
        this.enemy = new Entity(enemyHealth);
    }
}