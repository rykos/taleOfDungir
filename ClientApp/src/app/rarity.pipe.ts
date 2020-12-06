import { Rarity } from './../_models/Rarity';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'rarity'
})
export class RarityPipe implements PipeTransform {

  transform(value: string, ...args: unknown[]): unknown {
    return Rarity[value];
  }

}
