export interface IVoidResult {
    success: boolean
    errors: string[]
}

export interface IResult<T> extends IVoidResult {
    data: T
}
