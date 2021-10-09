<template>
    <div class="modal">
        <div class="modal-content">
            <div class="modal-content__head">
                <div class="modal-content__head__title">
                    {{ titleText }}
                </div>
                <button 
                    @click="$emit('modal-close')" 
                    class="modal-content__head__close">
                    Close X
                </button>
            </div>
            <slot></slot>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        titleText: String
    },
})
</script>

<style lang="less" scoped>
.modal {
    position: fixed;
    top: 0;
    left: 0;

    width: 100%;
    height: 100%;
    
    background: rgba(36, 36, 36, 0.4);
}


.modal-content {
    position: absolute;
    width: 60%;
    top: 10%;
    left: 20%;

    display: flex;
    flex-direction: column;

    background-color: white;

    &__head {
        flex: 0 0 10%;
        display: flex;
        justify-content: space-between;

        border-bottom: 1px solid black;

        .shared {
            padding: 15px;
            user-select: none
        }

        &__title {
            .shared;
        }

        &__close {
            .shared;

            border: 0;
            background: 0;
            outline: 0;

            &:hover {
                cursor: pointer;
                background-color: lightgray;
            }
        }
    }
}
</style>

