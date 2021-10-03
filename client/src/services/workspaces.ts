import axios, { AxiosResponse } from 'axios';

import { IResult, IVoidResult, generateErrorMessages } from '@/types/IResult';
import { IWorkspace } from '@/types/IWorkspace';

const WORKSPACES_SUFFIX = '/_workspaces';

export async function createWorkspace(name: string, description?: string): Promise<IVoidResult> {
    const result: IVoidResult = {
        success: false,
        errors: []
    };
    
    try {
        await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`, {
            name: name,
            description: description
        });

        result.success = true;
    } catch({ response }) {
        result.errors = generateErrorMessages(response.data);
    }

    return result;
}

export async function updateWorkspace(id: string, name: string, description?: string): Promise<IVoidResult> {
    const result: IVoidResult = {
        success: false,
        errors: []
    };

    try {
        await axios.put(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`, {
            name: name,
            description: description
        }, {
            params: {
                id: id 
            } 
        });

        result.success = true;
    } catch({ response }) {
        result.errors = generateErrorMessages(response.data);
    }

    return result;
}

export async function getWorkspaces(): Promise<IResult<IWorkspace[]>> {
    const result: IResult<IWorkspace[]> = {
        success: false,
        errors: [],
        data: [
            {
                id: '',
                name: '',
                description: ''
            }
        ]
    }
    try {
        const response: AxiosResponse<IWorkspace[]> = await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`);
        
        result.success = true;
        result.data = response.data;
    } catch({ response }) {
        result.errors = generateErrorMessages(response.data);
    }

    return result;
}
