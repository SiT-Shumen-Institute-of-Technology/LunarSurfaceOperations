import axios, { AxiosResponse } from 'axios';

import { IBearer } from '@/types/IBearer';
import { makeRequest } from '@/services/API/base';
import { IResult, IVoidResult } from '@/types/IResult';

const AUTH_SUFFIX = '/_auth';

export async function register(username: string, email: string, password: string): Promise<IVoidResult> {
    const result: IResult<void> = await makeRequest<void>(async () => {
        return await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${AUTH_SUFFIX}/signup`, {
            username: username,
            email: email, 
            password: password
        });
    });

    return result;
}

export async function signin(username: string, password: string): Promise<IResult<IBearer>> {
    const result: IResult<IBearer> = await makeRequest<IBearer>(async () => {
        return await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${AUTH_SUFFIX}/signin`, {
            username: username,
            password: password
        });
    });

    return result;
}
