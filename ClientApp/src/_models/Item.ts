import { Stats } from './Stats';
import { ItemType } from './ItemType';
export class Item {
    itemId?: number;
    level: number;
    name: string;
    power: number;
    value: number;
    itemType?: ItemType;
    iconID?: number;
    worn?: boolean;
    stats?: Stats;
}