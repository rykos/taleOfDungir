import { Time } from './../_models/Time';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time'
})
export class TimePipe implements PipeTransform {

  transform(value: Time, ...args: unknown[]): string {
    return `H:${value.hours} M:${value.minutes} S:${value.seconds}`;
  }

}
