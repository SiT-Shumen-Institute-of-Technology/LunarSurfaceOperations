<template>
    <div v-if="isSignedUp && workspaces.length !== 0" 
        class="main-workspaces">
        <div class="workspace" v-for="workspace in workspaces" :key="workspace">
            <WorkspaceSidePane :id="workspace.id" :name="workspace.name" :description="workspace.description" /> 
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, onBeforeMount } from 'vue'

import { useAuthState } from '@/utils/globalUtils'
import { useWorkspaces } from '@/composables/state/globalState';
import WorkspaceSidePane from '@/components/WorkspaceSidePane.vue';

export default defineComponent({
    components: {
        WorkspaceSidePane,
    },
    setup() {
        const [ isSignedUp ] = useAuthState();
        const { fetchWorkspaces, workspaces } = useWorkspaces();

        onBeforeMount(async () => {
            if (isSignedUp.value) {
                await fetchWorkspaces();
            }
        });

        return {
            isSignedUp,
            workspaces: computed(() => workspaces.value)
        }
    }
})
</script>

<style lang="less" scoped>
    .main-workspaces {
        border-right: 1px solid black;
    }
</style>
