<template>
    <div v-if="attribute.attributeName === 'subject'"
        class="attribute__subject">
        Subject: {{ attribute.value }}
    </div>

    <div v-if="attribute.attributeName === 'date'"
        class="attribute__date">
        Addition date: {{ attribute.value }}
    </div>

    <div v-if="attribute.attributeName === 'description'"
        class="attribute__description">
        Addition description: {{ attribute.value }}
    </div>

    <div v-if="attribute.attributeName === 'funnyMessage'" 
        class="attribute__funnyMessage">
        <sub>
            <em>
                {{ attribute.value }}
            </em>
        </sub>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
    props: {
        attribute: {
            type: Object,
            default() {
                return {
                    value: '',
                    attributeName: '',
                    type: '' 
                }
            }
        }
    }
})
</script>

<style lang="less" scoped>
.attribute__funnyMessage {
    font-size: 15px;
    color: var(--attribute__funnyMessage-txt-color, black);
}
</style>
