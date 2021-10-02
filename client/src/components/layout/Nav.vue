<template>
    <nav class="main-nav">
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
        justify-content: flex-end;

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

