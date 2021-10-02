<template>
    <div class="chat-wrapper">
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import * as signalr from '@microsoft/signalr';

export default defineComponent({
    setup() {
        const bearer: string = window.localStorage.getItem('JTW') || '';
        const con = new signalr.HubConnectionBuilder()
        .withUrl("http://localhost:5000/_hubs/messages", { accessTokenFactory: () => bearer })
        .build();

        con.start();
    },
})
</script>

