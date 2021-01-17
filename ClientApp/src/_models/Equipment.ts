import { ItemType } from './ItemType';
import { Item } from './Item';
export class Equipment {
    head?: Item;
    body?: Item;
    legs?: Item;
    neck?: Item;
    finger?: Item;
    weapon?: Item;

    constructor(items: Item[]) {
        items = items.filter(i => i.worn);
        this.head = items.find(i => i.itemType == ItemType.Head);
        this.body = items.find(i => i.itemType == ItemType.Body);
        this.legs = items.find(i => i.itemType == ItemType.Legs);
        this.neck = items.find(i => i.itemType == ItemType.Neck);
        this.finger = items.find(i => i.itemType == ItemType.Finger);
        this.weapon = items.find(i => i.itemType == ItemType.Weapon);
    }
}