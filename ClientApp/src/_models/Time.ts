export class Time {
    hours: number;
    minutes: number;
    seconds: number;
    totalSeconds: number;

    public constructor(seconds: number) {
        this.totalSeconds = seconds;
        this.hours = Math.floor(seconds / 3600);
        seconds -= this.hours * 3600;
        this.minutes = Math.floor(seconds / 60);
        seconds -= this.minutes * 60;
        this.seconds = seconds;
    }
}