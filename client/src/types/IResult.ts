export interface IVoidResult {
    success: boolean
    errors: string[]
}

export interface IResult<T> extends IVoidResult {
    data?: T
}

export function generateErrorMessages(errors: string): string[] {
    return errors.trimEnd().split('\n');
}
