<template>
    <div class="modal">
        <slot></slot>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
})
</script>

<style lang="less" scoped>
.modal {
    position: fixed;
    top: 0;
    left: 0;

    width: 100%;
    height: 100%;
    
    background: rgba(36, 36, 36, 0.4);
}
</style>

