import * as signalr from '@microsoft/signalr';

import { getJWT } from '@/utils/globalUtils';
import Vue from 'vue';

const connection: signalr.HubConnection | null = null;

export async function useSignalR(workspaceId: string, newMessageCallback: any): Promise<void> {

    const connection = new signalr.HubConnectionBuilder()
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

    try {
        const invoked = await connection.invoke('ConnectToWorkspace', workspaceId);
        console.log(invoked);
    } catch(error) {
        console.log(error);
    }
}

export async function resetSignalR(): Promise<void> {
    try {
        await connection?.invoke('DisconnectFromWorkspace');
    } catch (error) {
        console.log(error);
    }
}
