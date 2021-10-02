<template>
    <div class="error-wrapper" v-for="error in errors" :key="error">
        <div class="error"> {{ error }} </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        errors: Array
    },
})
</script>

