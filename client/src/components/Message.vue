<template>
    <div :id="id" class="message" :class="{ 'isApproved': status === 1 }">
        [{{ time }}]
        {{ author.username }}:
        {{ text }}

        <div>
            <span v-for="attribute in attributes" :key="attribute"> 
                {{ attribute.attributeName }}: {{ attribute.value }}<br>
            </span>
        </div>
        <button v-if="username === author.username && !status" @click="approve">Approve</button>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { approveMessage } from '@/services/API/messages';

export default defineComponent({
    props: {
        workspaceId: String,
        id: String,
        text: String,
        author: Object,
        attributes: Array,
        timestamp: Number,
        status: Number
    },
    setup(props) {
        const approve = async () => {
            await approveMessage(props.workspaceId || '', props.id || '');
        };
        return {
            time: new Date(props.timestamp || '').toLocaleString(),
            username: window.localStorage.getItem('username'),
            approve
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
        padding: 5px 10px;
    }

    .message:not(:last-of-type) {
        border-bottom: 1px solid black;
    }

    .message.isApproved {
        background-color: rgba(77, 175, 124, 0.5) ;

        color: white;
    }

    .message.sent-by-me {
        text-align: right;
    }
</style>
