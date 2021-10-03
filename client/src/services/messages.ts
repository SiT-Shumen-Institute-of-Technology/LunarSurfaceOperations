import axios, {AxiosResponse} from "axios";

import {generateErrorMessages, IResult, IVoidResult} from "@/types/IResult";
import { IMessage } from '@/types/IMessage';
import {IWorkspace} from "@/types/IWorkspace";

const MESSAGES_SUFFIX = '/_messages';

export async function sendMessage(message: IMessage, workspace: string): Promise<IVoidResult> {
    const result: IVoidResult = {
        success: false,
        errors: []
    };

    try {
        await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${MESSAGES_SUFFIX}`, message, {
            params: {
                workspaceId: workspace
            }
        });

        result.success = true;
    } catch ({ response }) {
        result.errors = generateErrorMessages(response.data);
    }

    return result;
}

export async function getMessages(workspaceId: string): Promise<IResult<IMessage[]>> {
    const result: IResult<IMessage[]> = {
        success: false,
        errors: [],
        data: [
            {
                text: ''
            }
        ]
    };

    try {
        const getResult: AxiosResponse<IMessage[]> = await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${MESSAGES_SUFFIX}`, {
            params: {
                workspaceId: workspaceId
            }
        });

        result.data = getResult.data;
        result.success = true;
    } catch ({ response }) {
        result.errors = response.data;
    }

    return result;
}
