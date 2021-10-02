<template>
    <nav class="main-nav">
        <div class="nav-left">
            <router-link v-if="isSignedIn" class="link" to="/workspace_create">+ Add workspace</router-link>
        </div>
        <div class="nav-right">
            <span v-if="isSignedIn"> {{ username }} </span>
            <router-link v-if="!isSignedIn" class="link" to="/signin">Sign In</router-link>
            <router-link v-if="!isSignedIn" class="link" to="/register">Register</router-link>
            <router-link v-if="isSignedIn" class="link" to="/home" @click="signOut">Sign out</router-link>
        </div>
    </nav>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import { useAuthState } from '@/utils/globalUtils';

export default defineComponent({
    setup() {
        const [ isSignedIn, _, setSingnedOut, username ] = useAuthState();
        const signOut = () => {
            window.localStorage.removeItem('JWT');
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
        display: flex;

        border-bottom: 1px solid black;
        justify-content: space-between;

        .link {
            padding: 20px;
            text-decoration: none;
            color: black;
            display: inline-block;

            &:hover {
                background-color: lightblue;
            }
        }
    }
</style>

