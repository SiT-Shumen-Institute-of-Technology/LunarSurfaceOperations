<template>
    <div class="chat-message-box">
        <div class="chat-message-box__input">
            <CustomInput 
                @keypress.enter.prevent="send"
                :maxWidth="true"
                :opaque="true"
                placeholder="message" 
                v-model:input="inputValue" type="text"
            />
        </div>
        <div class="chat-message-box__attributes-button">
            <button
                @click="toggleAttributesModal" class="button">
                More 
            </button>
        </div>

        <Modal v-if="showAttributesModal"
            titleText="Addition Details"
            @modalClose="toggleAttributesModal">
            <div class="modal-content__body">
                <CustomInput 
                    :maxWidth="true"
                    :opaque="false"
                    placeholder="Subjcet"
                    type="text"
                    v-model:input="additionalInfo.subject"
                />
                <CustomInput 
                    :maxWidth="true"
                    :opaque="false"
                    placeholder="Date"
                    type="date"
                    v-model:input="additionalInfo.date"
                />
                <CustomInput 
                    :maxWidth="true"
                    :opaque="false"
                    placeholder="Additional description"
                    type="text"
                    v-model:input="additionalInfo.description"
                />
                <CustomInput 
                    :maxWidth="true"
                    :opaque="false"
                    placeholder="Funny little message"
                    type="text"
                    v-model:input="additionalInfo.funnyMessage"
                />
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from 'vue'

import CustomInput from '@/components/CustomInput.vue';
import Modal from '@/components/Modal.vue';

import { IAttribute, IMessage } from '@/types/IMessage';
import { sendMessage } from '@/services/API/messages';

export default defineComponent({
    components: {
        CustomInput,
        Modal
    },
    props: {
        workspaceId: String
    },
    setup(props) {
        const inputValue: Ref<string> = ref('');
        const showAttributesModal: Ref<boolean> = ref(false);

        const additionalInfo: Ref<any> = ref({
            subject: '',
            description: '',
            date: '',
            funnyMessage: ''
        });

        const send = async () => {
            if (inputValue.value !== '') {
                let attrs: IAttribute[] = [];
                const additionalInfoKeys = Object.keys(additionalInfo.value);
                for (const key of additionalInfoKeys) {
                    if (additionalInfo.value[key] !== '') {
                        attrs.push({
                            type: 'string',
                            attributeName: key,
                            value: additionalInfo.value[key]
                        });
                    }
                }

                const message: IMessage = {
                    text: inputValue.value,
                    attributes: attrs 
                };

                const result = await sendMessage(message, props.workspaceId || '');

                if (result.success) {
                    inputValue.value = '';

                    additionalInfo.value = {
                        subject: '',
                        description: '',
                        date: '',
                        funnyMessage: ''
                    };
                }
            }
        }

        const toggleAttributesModal = () => {
            showAttributesModal.value = !showAttributesModal.value;
        };

        return {
            inputValue,
            sendMessage,
            send,
            showAttributesModal,
            toggleAttributesModal,
            additionalInfo
        }
    }
})
</script>

<style lang="less" scoped>
.chat-message-box {
    display: grid;
    grid-template-columns: repeat(16, 1fr);

    &__input {
        grid-column-end: span 14;
        width: 100%;
    }

    &__attributes-button {
        grid-column-end: span 2;
        
        .button {
            width: 100%;
            height: 100%;

            outline: none;
            border: 0;

            background-color: green;
            color: white;

            &:hover {
                cursor: pointer;
            }
        }
    }
}

.modal-content__body {
    flex: 0 0 90%;
    padding: 15px;

    .field:not(:first-of-type) {
        margin-top: 10px;
    }
}
</style>
