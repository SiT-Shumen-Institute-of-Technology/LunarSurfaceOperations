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
    body {
        margin: 0;
    }

    button {
        background-color: unset;
        outline: none;
        border: 0;
        padding: 20px;

        &:hover {
            background-color: lightblue;
        }
    }

    .flex {
        display: flex;
    }


    .main-content {
        .flex;

        .main-workspaces {
            flex: 20%;
            overflow-y: auto;

            button {
                width: 100%;
                text-align: left;
            }
        }

        .main-content-placeholder {
            flex: 100%;

            .main-workspaces ~ .main-content-placeholder {
                flex: 80%;
            }
        }
    }
</style>
