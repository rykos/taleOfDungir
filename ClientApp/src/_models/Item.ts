import { ItemType } from './ItemType';
export class Item {
    id?: number;
    level: number;
    name: string;
    power: number;
    value: number;
    itemType?: ItemType;
}