export class ApiError extends Error {
    constructor(public readonly errors: string[]) {
        super();
    }

    public get message() {
        return this.errors[0];
    }
}