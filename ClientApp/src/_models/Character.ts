import { Item } from './Item';
import { LifeSkills } from './LifeSkills';
import { Skills } from './Skills';
export class Character {
    exp: number;
    gold: number;
    health: number;
    level: number;
    skills: Skills;
    lifeSkills: LifeSkills;
    inventory?: Item[];
}