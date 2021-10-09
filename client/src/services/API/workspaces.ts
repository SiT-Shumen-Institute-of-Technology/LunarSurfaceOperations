import axios from 'axios';

import { IWorkspace } from '@/types/IWorkspace';
import { makeRequest } from '@/services/API/base';
import { IResult, IVoidResult } from '@/types/IResult';
import {IUser} from '@/types/IUser';

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

export async function putMemebers(workspaceId: string, usernames: string[]): Promise<IResult<void>> {
    const result: IResult<void> = await makeRequest<void>(async() => {
        return await axios.put(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}/members`, {
            members: usernames
        }, {
            params: {
                id: workspaceId
            }
        });
    });

    return result;
}

export async function getMembers(id: string): Promise<IResult<IUser[]>> {
    const result: IResult<IUser[]> = await makeRequest<IUser[]>(async () => {
        return await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}/members`, {
            params: {
                id: id
            }
        });
    });

    return result;
}
