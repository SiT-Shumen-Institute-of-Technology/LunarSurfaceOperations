<template>
    <div v-if="isSignedIn && workspaces.length !== 0" 
        class="main-workspaces">
        <WorkspaceConnections :workspaces="workspaces" />
    </div>
</template>

<script lang="ts">
import { defineComponent, computed, onBeforeMount } from 'vue'

import { 
    useWorkspaces,
    useAuthState    
} from '@/composables/state/globalState';

import WorkspaceConnections from '@/components/WorkspaceConnections.vue';

export default defineComponent({
    components: {
        WorkspaceConnections
    },
    setup() {
        const { isSignedIn } = useAuthState();
        const { fetchWorkspaces, workspaces } = useWorkspaces();

        onBeforeMount(async () => {
            if (isSignedIn.value) {
                await fetchWorkspaces();
            }
        });

        return {
            isSignedIn,
            workspaces: computed(() => workspaces.value)
        }
    }
})
</script>

<style lang="less">
.main-workspaces {
    border-right: 1px solid black;
    box-sizing: border-box;
}
</style>
