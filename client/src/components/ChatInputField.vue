<template>
    <div class="input-section__wrapper">
        <div class="input-section__field">
            <input @keypress.enter="send" v-model="inputValue"
                placeholder="Cool message here" 
                class="input-section__input" 
                type="text" />
        </div>
        <div class="input-section__more-attributes">
            <button @click="toggleAttributesModal" class="button"> More </button>
        </div>

        <div v-if="showAttributesModal"
            class="input-section__attributes-modal">
            <div class="input-section__attributes-modal__body">
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from 'vue'

import { IAttribute, IMessage } from '@/types/IMessage';
import { sendMessage } from '@/services/API/messages';

export default defineComponent({
    props: {
        workspaceId: String
    },
    setup(props) {
        const inputValue: Ref<string> = ref('');
        const showAttributesModal: Ref<boolean> = ref(false);

        const send = async () => {
            if (inputValue.value !== '') {
                let attrs: IAttribute[] = [];

                const message: IMessage = {
                    text: inputValue.value,
                    attributes: attrs 
                };

                const result = await sendMessage(message, props.workspaceId || '');

                if (result.success) {
                    inputValue.value = '';
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
            toggleAttributesModal
        }
    }
})
</script>

<style lang="less" scoped>
.input-section {
    &__wrapper {
        display: grid;
        grid-template-columns: repeat(16, 1fr);
    }

    &__field {
        grid-column-end: span 14;
    }

    &__input {
        width: 100%;
        height: 30px;

        padding: 0;
        outline: 0;
        border: 0;
    }

    &__more-attributes {
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

    &__attributes-modal {
        // TODO(n): the modal
    }
}
</style>
