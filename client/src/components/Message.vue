<template>
    <div :id="id" class="message">
        [{{ time }}]
        {{ author.username }}:
        {{ text }}

        <div>
            <span v-for="attribute in attributes" :key="attribute"> 
                {{ attribute.attributeName }}: {{ attribute.value }}
            </span>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        id: String,
        text: String,
        author: Object,
        attributes: Array,
        timestamp: Number
    },
    setup(props) {
        return {
            time: new Date(props.timestamp || '').toLocaleString(),
        }
    }
})
</script>

<style lang="less" scoped>
    .message {
        height: auto;
        background-color: rgba(250, 250, 250, 0.5);
        word-break: break-all;
        font-size: 20px;
    }

    .message:not(:last-of-type) {
        border-bottom: 1px solid black;
    }

    .message.sent-by-me {
        text-align: right;
    }
</style>
