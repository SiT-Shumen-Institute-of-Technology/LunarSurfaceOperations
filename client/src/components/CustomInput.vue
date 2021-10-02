<template>
    <div class="field">
        <div v-if="label">{{ label }}</div>
        <input class="custom-input" :type="type" v-model="value" @change="$emit('update-value', value)" />    
    </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'

export default defineComponent({
    props: {
        type: String,
        label: String,
    },
    setup() {
        const value = ref('');

        return {
            value,
        }
    }
})
</script>

<style lang="less" scoped>
    .field {
        .custom-input {
            width: 100%;
        }
    }
</style>

