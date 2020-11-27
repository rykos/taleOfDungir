export class User {
    id: string;
    admin: boolean;
    username: string;
    expiration: any;
    token?: string;
    validTo?: Date;
}