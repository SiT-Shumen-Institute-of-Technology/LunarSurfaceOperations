import axios, { AxiosResponse } from 'axios';

import { IWorkspace } from '@/types/IWorkspace';
import { makeRequest } from '@/services/API/base';
import { IResult, IVoidResult } from '@/types/IResult';

const WORKSPACES_SUFFIX = '/_workspaces';

export async function createWorkspace(name: string, description?: string): Promise<IVoidResult> {
    const result: IResult<void> = await makeRequest<void>(async() => {
        return await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`, {
            name: name,
            description: description
        });
    });
    
    return result;
}

export async function updateWorkspace(id: string, name: string, description?: string): Promise<IVoidResult> {
    const result: IResult<void> = await makeRequest<void>(async() => {
        return await axios.put(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`, {
            name: name,
            description: description
        }, {
            params: {
                id: id 
            } 
        });
    });

    return result;
}

export async function getWorkspaces(): Promise<IResult<IWorkspace[]>> {
    const result: IResult<IWorkspace[]> = await makeRequest<IWorkspace[]>(async() => {
        return await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`);
    });

    return result;
}
