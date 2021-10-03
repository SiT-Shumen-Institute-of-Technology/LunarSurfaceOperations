<template>
    <Nav />
    <div class="main-content">
        <SidePane />
        <div class="main-content-placeholder">
            <router-view/>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import Nav from './components/layout/Nav.vue';
import SidePane from './components/layout/SidePane.vue';

export default defineComponent({
    components: {
        Nav,
        SidePane
    },
})
</script>

<style lang="less">
    html, body {
        margin: 0;
        height: 100%;
    }

    body:before {
        content: ''; 
        display: block;
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        opacity: 0.6;
        background-image: url('./assets/background.jpg');

        background-size: 100% 100%;
        background-repeat: no-repeat;
        background-position: center;
    }

    #app {
        height: 100%;
        display: flex;
        flex-flow: column;
        position: relative;
    }

    .flex {
        display: flex;
    }

    .main-content {
        .flex;
        flex: 1 1 auto;
        overflow-y: scroll;

        .main-workspaces {
            flex: 20%;
            overflow-y: auto;
        }

        .main-content-placeholder {
            flex: 100%;

            .main-workspaces ~ .main-content-placeholder {
                flex: 80%;
            }
        }
    }

    .link {
        padding: 20px;
        text-decoration: none;
        color: black;
        display: inline-block;
        pointer-events: all;

        &:hover {
            background-color: lightblue;
        }
    }
</style>
