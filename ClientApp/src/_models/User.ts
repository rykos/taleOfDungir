export class User {
    id: string;
    admin: boolean = false;
    username: string;
    expiration: any;
    token?: string;
    validTo?: Date;
}