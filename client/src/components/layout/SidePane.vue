<template>
    <div v-if="isSignedUp && workspaces.length !== 0" 
        class="main-workspaces">
        <div class="workspace" v-for="workspace in workspaces" :key="workspace">
            <WorkspaceSidePane :id="workspace.id" :name="workspace.name" :description="workspace.description" /> 
        </div>
    </div>
</template>

<script lang="ts">
import { useStore } from 'vuex';
import { defineComponent, onMounted, computed } from 'vue'

import { useAuthState } from '@/utils/globalUtils'
import WorkspaceSidePane from '@/components/WorkspaceSidePane.vue';

export default defineComponent({
    components: {
        WorkspaceSidePane,
    },
    setup() {
        const [isSignedUp] = useAuthState();
        const store = useStore();

        onMounted(async () => {
            store.dispatch('fetchWorkspaces');
        });

        return {
            isSignedUp,
            workspaces: computed(() => store.state.workspaces)
        }
    }
})
</script>

<style lang="less" scoped>
    .main-workspaces {
        border-right: 1px solid black;
    }
</style>
