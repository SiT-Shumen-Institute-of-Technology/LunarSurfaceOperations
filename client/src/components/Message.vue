<template>
    <div :id="id" class="message" :class="{ 'isApproved': status === 1 }">
        <div class="message__time">
            <em>[{{ time }}]</em>
        </div>
        
        <div class="message__author__text">
            <span class="author">
                {{ author.username }}: 
            </span>
            <span class="text">
                {{ text }}
            </span>
        </div>

        <div class="message__attributes">
            <div v-for="attribute in attributes" :key="attribute" 
                class="attribute">
                <Attribute :attribute="attribute" />
            </div>
        </div>
        
        <div class="message__approve">
            <button v-if="username === author.username && !status" @click="approve">Approve</button>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { approveMessage } from '@/services/API/messages';

import Attribute from '@/components/Attribute.vue';

export default defineComponent({
    components: {
        Attribute
    },
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
        background-color: var(--message--bg-color, rgba(250, 250, 250, 0.5));
        word-break: break-all;
        font-size: 20px;
        padding: 5px 10px;

        &__time {
            font-size: 10px;
            color: var(--message__time--txt-color, black)
        }
    }

    .message:not(:last-of-type) {
        border-bottom: 1px solid black;
    }

    .message.isApproved {
        background-color: var(--message--bg-color--approved ,rgba(77, 175, 124, 0.5));

        color: var(--message--txt-color--approved, white);
    }
</style>
