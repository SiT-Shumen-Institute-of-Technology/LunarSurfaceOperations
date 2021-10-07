<template>
  <div class="home">
  </div>
</template>

<script lang="ts">
import { defineComponent } from '@vue/runtime-core';

export default defineComponent({
  name: 'Home',
});
</script>

<style lang="less" scoped>
    .home {
        position: absolute;
        text-align: center;
        font-size: 70px;
        font-weight: bold;
        left: 0;
        right: 0;
        top: 40%;
        color: white
    }
</style>
