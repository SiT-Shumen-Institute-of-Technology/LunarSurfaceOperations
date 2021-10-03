<template>
    <div class="input-section-wrapper">
        <div class="input-field">
            <input v-model="inputValue" class="input" type="text" />
        </div>
        <div class="input-send">
            <button @click="send" class="send">Send</button>
        </div>
    </div>
    
    <div class="add-username">
        <button @click="toggleUsernameAttribute"> 
            Subject Attribute
        </button>

        <input v-if="showUsernameAttr" type="text" v-model="usernameAttr" />
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from 'vue'

import { IAttribute, IMessage } from '@/types/IMessage';
import { sendMessage } from '@/services/messages';

export default defineComponent({
    props: {
        workspaceId: String
    },
    setup(props) {
        const inputValue: Ref<string> = ref('');
        const showUsernameAttr: Ref<boolean> = ref(false);
        const usernameAttr: Ref<string> = ref('');

        const send = async () => {
            if (inputValue.value !== '') {
                const attrs: IAttribute[] = [
                    { type: 'string', value: usernameAttr.value, attributeName: 'subject' }
                ];

                const message: IMessage = {
                    text: inputValue.value,
                    attributes: attrs 
                };

                const result = await sendMessage(message, props.workspaceId || '');

                console.log(result);
                if (result.success) {
                    inputValue.value = '';
                    // TODO(n): add it to the chat
                }
            }
        }

        const toggleUsernameAttribute = () => {
            showUsernameAttr.value = !showUsernameAttr.value;
        }

        return {
            inputValue,
            sendMessage,
            send,
            toggleUsernameAttribute,
            showUsernameAttr,
            usernameAttr
        }
    }
})
</script>

