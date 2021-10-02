<template>
    <div class="chat-wrapper">
        <div class="input-section-wrapper">
            <div class="input-field">
                <input class="input" type="text" />
            </div>
            <div class="input-send">
                <button class="send">Send</button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue'
import * as signalr from '@microsoft/signalr';
import { onBeforeRouteUpdate, useRoute } from 'vue-router'
import { getJWT } from '@/utils/globalUtils';

export default defineComponent({
    setup() {
        const { params: { id } } = useRoute();
        const mainId = ref('');
        mainId.value = id.toString();

    
        onBeforeRouteUpdate((to, _, next) => {
            mainId.value = to.params.id.toString();
            next();
        });

        // TODO(n): signalr connection here

        const connection = new signalr.HubConnectionBuilder()
            .withUrl('http://localhost:5000/_hubs/messages', { accessTokenFactory: () => getJWT() || '' }).build();

        connection.start()
        .then(() => {
            connection.send('ConnectToWorkspace', mainId.value)
        } );

        return {
            mainId
        }
    },
})
</script>

<style lang="less" scoped>
    .chat-wrapper {
        .input-section-wrapper {
            display: flex;

            .input-field {
                flex: 0 0 90%;

                .input {
                    width: 100%;
                }
            }

            .input-send {
                flex: 0 0 10%;

                .send {
                    width: 100%;
                }
            }
        }
    }
</style>
