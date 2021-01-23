import { Time } from './../_models/Time';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time'
})
export class TimePipe implements PipeTransform {

  transform(value: Time, ...args: unknown[]): string {
    let res = "";
    if (value.hours) res += `H:${value.hours}`;
    if (value.minutes) res += `M:${value.minutes}`;
    if (value.seconds) res += `S:${value.seconds}`;
    return res;
  }

}
