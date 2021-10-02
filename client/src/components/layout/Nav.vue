<template>
    <nav class="main-nav">
        <div class="nav-left">
            <router-link v-if="isSignedIn" class="link" to="/workspace_create">+ Add workspace</router-link>
        </div>
        <div class="nav-right">
            <span v-if="isSignedIn"> {{ username }} </span>
            <router-link v-if="!isSignedIn" class="link" to="/signin">Sign In</router-link>
            <router-link v-if="!isSignedIn" class="link" to="/register">Register</router-link>
            <router-link v-if="isSignedIn" class="link" to="/" @click="signOut">Sign out</router-link>
        </div>
    </nav>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { useAuthState, removeJWT } from '@/utils/globalUtils';

export default defineComponent({
    setup() {
        const [ isSignedIn, _, setSingnedOut, username ] = useAuthState();
        const signOut = () => {
            removeJWT();
            window.localStorage.removeItem('username');
            setSingnedOut();
        }

        return {
            signOut,
            isSignedIn,
            username
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

    }
</style>

