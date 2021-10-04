import {AxiosResponse} from "axios";

import {generateErrorMessages, IResult} from "@/types/IResult";

export async function makeRequest<T>(fn: any): Promise<IResult<T>> {
    try {
        const result: AxiosResponse<T> = await fn();

        return {
            success: true,
            errors: [],
            data: result.data
        };
    } catch({ response }) {
        return {
            success: false,
            errors: generateErrorMessages(response.data),
        }
    }
}

