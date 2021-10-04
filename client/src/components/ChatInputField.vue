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
        <button @click="toggleAttribute"> 
            Attribute
        </button>

        <input v-if="showAttributeInput" placeholder="Attribute name" type="text" v-model="subjectName" />
        <input v-if="showAttributeInput" placeholder="Attribute value" type="text" v-model="subjectValue" />
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
        const showAttributeInput: Ref<boolean> = ref(false);
        const subjectName: Ref<string> = ref('');
        const subjectValue: Ref<string> = ref('');

        const send = async () => {
            if (inputValue.value !== '') {
                let attrs: IAttribute[] = [];
                if (showAttributeInput.value) {
                    attrs = [
                        { type: 'string', value: subjectValue.value, attributeName: subjectName.value }
                    ];
                }

                const message: IMessage = {
                    text: inputValue.value,
                    attributes: attrs 
                };

                const result = await sendMessage(message, props.workspaceId || '');

                if (result.success) {
                    inputValue.value = '';
                    subjectName.value = '';
                    subjectValue.value = '';
                }
            }
        }

        const toggleAttribute = () => {
            showAttributeInput.value = !showAttributeInput.value;
        }

        return {
            inputValue,
            sendMessage,
            send,
            toggleAttribute,
            showAttributeInput,
            subjectName,
            subjectValue
        }
    }
})
</script>

