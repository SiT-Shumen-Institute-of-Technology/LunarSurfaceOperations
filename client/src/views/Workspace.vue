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

import { getMessages } from '@/services/API/messages';

import ChatInputField from '@/components/ChatInputField.vue';
import Message from '@/components/Message.vue';

import { connectToWorkspace, disconnectFromWorkspace } from '@/services/signalr/connectionHandler';
import { useCurrentWorkspaceMessages } from '@/composables/state/globalState';

export default defineComponent({
    components: {
        ChatInputField,
        Message
    },
    setup() {
        const { params: { id } } = useRoute();
        const { 
            setMessages, 
            currentConnectionMessages, 
        } = useCurrentWorkspaceMessages();


        const mainId = ref('');
        mainId.value = id.toString();

        const fetchMessages = async () => {
            const getMessagesResult = await getMessages(mainId.value);

            if (getMessagesResult.success && getMessagesResult.data) {
                setMessages(getMessagesResult.data);
            } else {
                // TODO(n): handle errors here
                console.log(...getMessagesResult.errors);
            }
        }

        onMounted(async () => {
            await fetchMessages();

            await connectToWorkspace(mainId.value);
        });

        onBeforeRouteUpdate(async (to, _, next) => {
            await disconnectFromWorkspace(mainId.value);

            mainId.value = to.params.id.toString();

            await connectToWorkspace(mainId.value);

            await fetchMessages();

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
    }
</style>
