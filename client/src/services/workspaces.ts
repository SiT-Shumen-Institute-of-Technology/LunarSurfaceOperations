// TODO(n): Replace any with the type
import axios from 'axios';

import {IResult, IVoidResult} from '@/types/IResult';

const WORKSPACES_SUFFIX = '/_workspaces';

export async function createWorksace(name: string, description?: string): Promise<IVoidResult> {
    const result: IVoidResult = {
        success: false,
        errors: []
    };
    
    try {
        await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`, {
            name: name,
            description: description
        });
    } catch(error) {
        result.errors.push(error);
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
    } catch(error) {
        result.errors.push(error);
    }

    return result;
}

export async function getWorkspaces(): Promise<IResult<any>> {
    const result: IResult<any> = {
        success: false,
        errors: [],
        data: {}
    }
    try {
        const response: any = await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${WORKSPACES_SUFFIX}`);
        
        result.success = true;
        result.data = response.data;
    } catch(error) {
        result.errors.push(error);
    }

    return result;
}
