<template>
    <div class="root-element">
        <Nav />
        <div class="main-content">
            <SidePane />
            <div class="main-content-placeholder">
                <router-view/>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import Nav from './components/layout/Nav.vue';
import SidePane from './components/layout/SidePane.vue';

import { IMessage } from '@/types/IMessage';
import { IWorkspace } from '@/types/IWorkspace';

import { useSignalR } from '@/services/signalr/connectionHandler';
import { useAuthState, useCurrentWorkspaceMessages, useWorkspaces } from '@/composables/state/globalState';

export default defineComponent({
    components: {
        Nav,
        SidePane
    },
    setup() {
        const { addWorkspace } = useWorkspaces();
        const { updateMessage, addMessage } = useCurrentWorkspaceMessages();
        const { isSignedIn } = useAuthState();

        const newMessage = (message: IMessage) => {
            addMessage(message);
        }

        const updateMessageLocal = (message: IMessage) => {
            updateMessage(message);
        }

        const updateWorkspace = (workspace: IWorkspace) => {
            addWorkspace(workspace);
        }

        if (isSignedIn.value) {
            useSignalR(newMessage, updateMessageLocal, updateWorkspace);
        }
    }
})
</script>

<style lang="less">
    html, body, #app {
        margin: 0;
        height: 100%;
        font-family: Arial, Helvetica, sans-serif; 
    }

    .root-element:before {
        content: ''; 
        display: block;
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        opacity: 0.6;
        z-index: -1;
        background-image: url('./assets/background.jpg');

        background-size: 100% 100%;
        background-repeat: no-repeat;
        background-position: center;
    }

    .root-element {
        height: 100%;
        display: flex;
        flex-flow: column;
        position: relative;
    }

    .flex {
        display: flex;
    }

    .main-content {
        max-height: 100%;
        overflow-y: auto;
        .flex;
        flex: 1 1 auto;

        .main-workspaces {
            flex: 0 0 20%;
            overflow-y: auto;
        }

        .main-content-placeholder {
            flex: 0 0 100%;

        }

        .main-workspaces ~ .main-content-placeholder {
            flex: 0 0 80%;
        }
    }

    .link {
        padding: 20px;
        text-decoration: none;
        color: black;
        display: inline-block;
        pointer-events: all;
        color: white;

        &:hover {
            background-color: lightblue;
            color: black;
        }
    }

    .form {
        display: grid;
        max-width: 50%;
        margin: 20px auto;

        grid-template-columns: repeat(2, 1fr);
        grid-auto-rows: minmax(40px, auto);

        &__field {
            grid-column-end: span 2;
        }

        &__submit {
            grid-column-end: span 1;
            
            border: 0;
            text-transform: uppercase;
            background-color: black;
            color: white;

            opacity: 0.9;

            &:hover {
                cursor: pointer;
            }
        }
    }

    .hide {
        display: none;
    }
</style>
