<template>
    <div class="chat-wrapper">
        <div class="chat">
            <Message v-for="message in messages" :key="message"
                :workspaceId="mainId"
                :id="message.id"
                :attributes="message.attributes"
                :timestamp="message.timestamp"
                :author="message.author"
                :status="message.status"
                :text="message.text" />
        </div>
        <ChatInputField :workspaceId="mainId" />
    </div>
</template>

<script lang="ts">
import { computed, defineComponent, onMounted, ref } from 'vue'
import { onBeforeRouteUpdate, useRoute } from 'vue-router'

import { getMessages } from '@/services/messages';

import ChatInputField from '@/components/ChatInputField.vue';
import Message from '@/components/Message.vue';

import { useSignalR, resetSignalR } from '@/services/signalr/connectionHandler';
import { useCurrentWorkspaceMessages, useWorkspaces } from '@/composables/state/globalState';
import { IMessage } from '@/types/IMessage';
import { IWorkspace } from '@/types/IWorkspace';

export default defineComponent({
    components: {
        ChatInputField,
        Message
    },
    setup() {
        const { params: { id } } = useRoute();
        const { 
            setMessages, 
            addMessage, 
            currentConnectionMessages, 
            updateMessage
        } = useCurrentWorkspaceMessages();

        const { addWorkspace } = useWorkspaces();

        const mainId = ref('');
        mainId.value = id.toString();

        const fetchMessages = async () => {
            const getMessagesResult = await getMessages(mainId.value);
            if (getMessagesResult.success) {
                setMessages(getMessagesResult.data);
            } else {
                // TODO(n): handle errors here
                console.log(...getMessagesResult.errors);
            }
        }

        const newMessage = (message: IMessage) => {
            addMessage(message);
        }

        const updateMessageLocal = (message: IMessage) => {
            updateMessage(message);
        }

        const updateWorkspaces = (workspace: IWorkspace) => {
            addWorkspace(workspace);
        }

        onMounted(async () => {
            await fetchMessages();
            useSignalR(mainId.value, newMessage, updateMessageLocal, updateWorkspaces);
        });

        onBeforeRouteUpdate(async (to, _, next) => {
            resetSignalR(mainId.value);
            mainId.value = to.params.id.toString();
            await fetchMessages();

            useSignalR(to.params.id.toString(), newMessage, updateMessageLocal, updateWorkspaces);

            next();
        });

        return {
            mainId,
            messages: computed(() => currentConnectionMessages.value),
        }
    },
})
</script>

<style lang="less">
    .chat-wrapper {
        display: flex;
        flex-direction: column; 
        height: 100%;

        .chat {
            display: flex;
            flex-direction: column;
            justify-content: flex-end;
            flex: 1 1 auto;
        }

        .input-section-wrapper {
            flex: 0 1 auto;
            display: flex;

            .input-field {
                flex: 0 0 90%;

                .input {
                    width: 100%;
                }
            }

            .input-send {
                flex: 0 0 10%;

                .send {
                    width: 100%;
                }
            }
        }
    }
</style>
