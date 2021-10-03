<template>
    <div :id="id" class="message" :class="{ 'sent-by-me': isMe }">
        {{ text }}
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        id: String,
        text: String,
        attributes: Array,
        isMe: Boolean
    },
})
</script>

<style lang="less" scoped>
    .message {
        height: auto;

        word-break: break-all;
    }

    .message:not(:last-of-type) {
        border-bottom: 1px solid black;
    }

    .message.sent-by-me {
        text-align: right;
    }
</style>
