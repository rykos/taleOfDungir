import { Equipment } from './Equipment';
import { Entity } from './Entity';
import { Item } from './Item';
import { LifeSkills } from './LifeSkills';
import { Skills } from './Skills';
export class Character {
    exp: number;
    reqExp: number;
    gold: number;
    level: number;
    skills: Skills;
    lifeSkills: LifeSkills;
    inventory?: Item[];
    equipment?: Equipment;
    entity?: Entity;
}