import * as signalr from '@microsoft/signalr';

import { getJWT } from '@/utils/globalUtils';
import Vue from 'vue';

let connection: signalr.HubConnection | null = null;

export async function useSignalR(workspaceId: string, newMessageCallback: any, updateMessageCallback: any): Promise<void> {
    connection = new signalr.HubConnectionBuilder()
        .withUrl(`${process.env.VUE_APP_API_ENDPOINT}/_hubs/messages` , {
            accessTokenFactory: () => getJWT() || "",
        })
        .withAutomaticReconnect()
        .configureLogging(signalr.LogLevel.Information)
        .build();

    await connection.start();

    connection.on('ReceiveMessage', (message: any) => {
        newMessageCallback(message);
        console.log('rec', message);
    });

    connection.on('UpdateMessage', (message: any) => {
        updateMessageCallback(message);
    });

    try {
        const invoked = await connection.invoke('ConnectToWorkspace', workspaceId);
        console.log(invoked);
    } catch(error) {
        console.log(error);
    }
}

export async function resetSignalR(workspaceId: string): Promise<void> {
    try {
        console.log(connection);
        await connection?.invoke('DisconnectFromWorkspace', workspaceId);
    } catch (error) {
        console.log(error);
    }
}
