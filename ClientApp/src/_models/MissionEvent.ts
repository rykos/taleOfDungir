import { EventAction } from './EventAction';
export class MissionEvent {
    id: number;
    name: string;
    description: string;
    imageLocation: string;
    eventActions: EventAction[];
}