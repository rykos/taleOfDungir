import { typeWithParameters } from "@angular/compiler/src/render3/util";

export class Entity {
    health: number;
    maxHealth: number;
    avatarID?: string;

    constructor(entityDTO: any) {
        this.health = entityDTO.health;
        this.maxHealth = entityDTO.maxHealth;
        this.avatarID = entityDTO.avatarID;
    }

    get HealthPercentage(): number {
        return (this.health / this.maxHealth) * 100;
    }
}