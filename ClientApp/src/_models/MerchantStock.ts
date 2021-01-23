import { Time } from './Time';
import { Item } from './Item';
export class MerchantStock {
    items: Item[];
    restockTime: Date;
    restockTimeRemaining?: Time;
}