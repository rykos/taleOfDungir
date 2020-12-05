import { Rarity } from './Rarity';
export class Mission {
    id: number;
    name: string;
    rarity: Rarity;
    duration: number;
    startTime?: Date;
}