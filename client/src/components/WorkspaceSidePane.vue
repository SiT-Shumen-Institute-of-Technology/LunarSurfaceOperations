<template>
    <button @click="redirect"
        class="description-button" :title="description"> {{ name }} </button>
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
</style>
