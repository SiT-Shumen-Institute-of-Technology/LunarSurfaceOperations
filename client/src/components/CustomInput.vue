<template>
    <div class="field">
        <input 
            :type="type"
            class="field__input field__input--swag-border" 
            :value="input"
            @input="$emit('update:input', $event.target.value)"
            @change="update"
            :class="{ 'field__input--full-width': maxWidth, 'field__input--opacity': opaque }"
        />
        <span 
            class="field__placeholder"
            v-if="placeholder"
            :class="{ 'field__placeholder--active': labelActive || type === 'date' }">
            {{ placeholder }}
        </span>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, Ref } from 'vue'

export default defineComponent({
    props: {
        maxWidth: Boolean,
        opaque: Boolean,
        placeholder: String,
        input: String,
        type: String
    },
    setup(props) {
        let isActiveLabelToStart = false;
        if (props.input) {
            isActiveLabelToStart = props.input.length > 0;
        }

        const labelActive: Ref<boolean> = ref(isActiveLabelToStart);

        const update = () => {
            if (props.input) {
                labelActive.value = props.input.length > 0; 
            }
        }

        return {
            labelActive,
            update
        } 
    }
});
</script>

<style lang="less" scoped>
    .input--resets {
        height: 55px;
        border: 1px solid transparent;
        outline: 0;
        margin: 0;
        padding: 15px 5px 0 5px;
        font-size: 20px;
        background-color: #f7f7f7;
        box-sizing: border-box;
    }

    .field {
        text-transform: uppercase;
        position: relative;

        .active {
            top: 6px;
        }

        &__input {
            .input--resets;

            &--full-width {
                width: 100%;
            }

            &--opacity {
                opacity: 0.8;
            }

            &--swag-border:hover,
            &--swag-border:focus {
                border: 1px solid black;
            }

            &:focus ~ .field__placeholder {
                .active;
            }
        }

        &__placeholder {
            position: absolute;
            top: 20px;
            left: 5px;
            pointer-events: none;
            color: gray;
            font-size: 12px;
            transition: 0.2s ease all;

            &--active {
                .active;
            }
        }
    }
</style>

