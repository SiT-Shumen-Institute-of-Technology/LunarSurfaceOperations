import * as signalr from '@microsoft/signalr';

import {IMessage} from '@/types/IMessage';
import {IWorkspace} from '@/types/IWorkspace';
import { getJWT } from '@/composables/state/globalState';

let connection: signalr.HubConnection | null = null;

export async function useSignalR(
    newMessageCallback: (message: IMessage) => void, 
    updateMessageCallback: (message: IMessage) => void, 
    updateWorkspace: (workspace: IWorkspace) => void): Promise<void> {
    connection = new signalr.HubConnectionBuilder()
        .withUrl(`${process.env.VUE_APP_API_ENDPOINT}/_hubs/messages` , {
            accessTokenFactory: () => getJWT() || "",
        })
        .withAutomaticReconnect()
        .configureLogging(signalr.LogLevel.Information)
        .build();

    await connection.start();

    connection.on('ReceiveMessage', (message: IMessage) => {
        newMessageCallback(message);
    });

    connection.on('UpdateMessage', (message: IMessage) => {
        updateMessageCallback(message);
    });

    connection.on('InviteToWorkspace', (workspace: any) => {
        updateWorkspace(workspace);
    });

    connection.on('RemoveFromWorkspace', (id: string) => {
        console.log(id);
    });
}

export async function connectToWorkspace(workspaceId: string): Promise<void> {
    try {
        await connection?.invoke('ConnectToWorkspace', workspaceId);
    } catch(error) {
        console.log(error);
    }
}

export async function disconnectFromWorkspace(workspaceId: string): Promise<void> {
    try {
        await connection?.invoke('DisconnectFromWorkspace', workspaceId);
    } catch (error) {
        console.log(error);
    }
}
