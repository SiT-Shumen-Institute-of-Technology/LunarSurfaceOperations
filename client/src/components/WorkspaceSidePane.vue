<template>
    <router-link class="link" :to="{ name: 'Workspace', params: { id } }">{{ name }}</router-link>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { useRouter } from 'vue-router'

export default defineComponent({
    props: {
        id: String,
        name: String,
        description: String || null
    },
    setup(props) {
        const router = useRouter();
        const redirect = () => {
            router.push(`/workspace/${props.id}`);
        }

        return {
            redirect
        }
    }
})
</script>

<style lang="less" scoped>
    .link {
        width: -webkit-fill-available;
        width: fill-available;
        width: -moz-available;
    }
</style>
