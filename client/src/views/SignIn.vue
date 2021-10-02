<template>
    <div class="register">
        <div class="register-wrapper">
            <CustomInput type="text" label="Username" @update-value="updateUsername" />

            <CustomInput type="password" label="Password" @update-value="updatePassword" />

            <button @click="submit">Submit</button>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, Ref, ref } from 'vue'
import { Router, useRouter } from 'vue-router';

import { useAuthState } from '@/utils/globalUtils';
import { signin } from '@/services/auth';
import { IResult } from '@/types/IResult';
import { IBearer } from '@/types/IBearer';

import CustomInput from '../components/CustomInput.vue';
import { useStore } from 'vuex';

export default defineComponent({
    components: {
        CustomInput
    },
    setup() {
        const store = useStore();
        const username: Ref<string> = ref('');
        const password: Ref<string> = ref('');
        const router: Router = useRouter();

        const updateUsername = (userInput: string) => {
            username.value = userInput;
        }

        const updatePassword = (passwordInput: string) => {
            password.value = passwordInput;
        }

        const submit = async () => {
            const registerResult: IResult<IBearer> = await signin(username.value, password.value);

            if (registerResult.success && registerResult.data.token) {
                const [ _, setSignedIn ] = useAuthState();
                window.localStorage.setItem('JWT', registerResult.data.token);
                window.localStorage.setItem('username', username.value);
                setSignedIn(username.value);
                store.dispatch('fetchWorkspaces');
                router.push('/home');
            }
        }
        return {
            updateUsername,
            updatePassword,
            submit
        }
    }
})
</script>

<style lang="less" scoped>
    .register {
        .register-wrapper {
            max-width: 50%;
            margin: 20px auto;
        }
    }
</style>
