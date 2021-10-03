<template>
    <div class="input-section-wrapper">
        <div class="input-field">
            <input v-model="inputValue" class="input" type="text" />
        </div>
        <div class="input-send">
            <button @click="send" class="send">Send</button>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from 'vue'

import { IMessage } from '@/types/IMessage';
import { sendMessage } from '@/services/messages';

export default defineComponent({
    props: {
        workspaceId: String
    },
    setup(props) {
        const inputValue: Ref<string> = ref('');

        const send = async () => {
            // TODO(n): future add attributes
            if (inputValue.value !== '') {
                const message: IMessage = {
                    text: inputValue.value
                };

                const result = await sendMessage(message, props.workspaceId || '');

                console.log(result);
                if (result.success) {
                    inputValue.value = '';
                    // TODO(n): add it to the chat
                }
            }
        }

        return {
            inputValue,
            sendMessage,
            send
        }
    }
})
</script>

