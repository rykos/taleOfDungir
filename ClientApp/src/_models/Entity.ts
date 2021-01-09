import { typeWithParameters } from "@angular/compiler/src/render3/util";

export class Entity {
    health: number;
    maxHealth: number;
    avatarId?: string;

    constructor(health: number, avatar?: string) {
        this.health = health;
        this.maxHealth = health;
        this.avatarId = avatar;
    }

    get HealthPercentage(): number {
        return (this.health / this.maxHealth) * 100;
    }
}