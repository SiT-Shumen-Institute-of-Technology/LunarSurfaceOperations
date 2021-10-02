import { IBearer } from '@/types/IBearer';
import { IResult, IVoidResult } from '@/types/IResult';
import axios, { AxiosResponse } from 'axios';

const AUTH_SUFFIX = '/_auth';

export async function register(username: string, email: string, password: string): Promise<IVoidResult> {
    const result: IVoidResult = {
        success: false,
        errors: []
    };

    try {
        await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${AUTH_SUFFIX}/signup`, {
            username: username,
            email: email, 
            password: password
        });

        result.success = true;
    } catch({ response }) {
        const s = response.data.split('\n');
        result.errors = s;
    }

    return result;
}

export async function signin(username: string, password: string): Promise<IResult<IBearer>> {
    const result: IResult<IBearer> = {
        success: false,
        errors: [],
        data: {
            token: null
        }
    } 
    try {
        const response: AxiosResponse<IBearer> = await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${AUTH_SUFFIX}/signin`, {
            username: username,
            password: password
        });

        result.success= true;
        result.data = response.data;
    } catch (error) {
        result.errors.push(error);
    }

    return result;
}
