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
        let percent = (this.health / this.maxHealth) * 100;
        if (percent < 0)
            percent = 0;
            
        return percent;
    }
}