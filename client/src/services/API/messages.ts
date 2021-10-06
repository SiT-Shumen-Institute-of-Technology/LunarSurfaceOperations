import axios from "axios";

import { IMessage } from '@/types/IMessage';
import { makeRequest } from '@/services/API/base';
import { IResult, IVoidResult } from "@/types/IResult";

const MESSAGES_SUFFIX = '/_messages';

export async function sendMessage(message: IMessage, workspace: string): Promise<IVoidResult> {
    const result: IResult<void> = await makeRequest<void>(async() => {
        return await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${MESSAGES_SUFFIX}`, message, {
            params: {
                workspaceId: workspace
            }
        });
    });

    return result;
}

export async function getMessages(workspaceId: string): Promise<IResult<IMessage[]>> {
    const result: IResult<IMessage[]> = await makeRequest<IMessage[]>(async() => {
        return await axios.get(`${process.env.VUE_APP_API_ENDPOINT}${MESSAGES_SUFFIX}`, {
            params: {
                workspaceId: workspaceId
            }
        });

    });

    return result;
}

export async function approveMessage(workspaceId: string, messageId: string): Promise<IVoidResult> {
    const result: IVoidResult = await makeRequest<void>(async() => {
        return await axios.post(`${process.env.VUE_APP_API_ENDPOINT}${MESSAGES_SUFFIX}/approve`, {
            messageId: messageId
        }, {
            params: {
                workspaceId: workspaceId
            }
        });
    });

    return result;
}
