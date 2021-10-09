<template>
    <nav class="main-nav">
        <div class="nav-left">
            <router-link v-if="isSignedIn" class="link" to="/workspace_create">+ Add workspace</router-link>
        </div>
        <div class="nav-right">
            <span class="username" v-if="isSignedIn"> {{ currentlyLoggedUsername }} </span>
            <router-link v-if="!isSignedIn" class="link" to="/signin">Sign In</router-link>
            <router-link v-if="!isSignedIn" class="link" to="/register">Register</router-link>
            <router-link v-if="isSignedIn" class="link" to="/" @click="signOut">Sign out</router-link>
        </div>
    </nav>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import {
    useCurrentWorkspaceMessages,
    useWorkspaces, 
    useAuthState, 
    removeJWT 
} from '@/composables/state/globalState';

export default defineComponent({
    setup() {
        const { isSignedIn, setSingnedOut, currentlyLoggedUsername } = useAuthState();
        const { setWorkspaces } = useWorkspaces();
        const { setMessages } = useCurrentWorkspaceMessages();
        const signOut = () => {
            removeJWT();
            window.localStorage.removeItem('username');

            setWorkspaces([]);
            setMessages([]);

            setSingnedOut();
        }

        return {
            signOut,
            isSignedIn,
            currentlyLoggedUsername
        }
    }
})
</script>

<style lang="less" scoped>
    .main-nav {
        flex: 0 1 auto;
        display: flex;

        border-bottom: 1px solid black;
        justify-content: space-between;
    
        .username {
            color: white
        }
    }
</style>

