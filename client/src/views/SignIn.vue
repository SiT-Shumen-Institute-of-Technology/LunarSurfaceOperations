<template>
    <div class="register">
        <div class="register-wrapper">
            
            <ErrorFields :errors="errors" />

            <CustomInput type="text" label="Username" @update-value="updateUsername" />

            <CustomInput type="password" label="Password" @update-value="updatePassword" />

            <button @click="submit">Submit</button>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, Ref, ref } from 'vue'
import { Router, useRouter } from 'vue-router';

import { useAuthState, setJWT } from '@/utils/globalUtils';
import { signin } from '@/services/API/auth';
import { IResult } from '@/types/IResult';
import { IBearer } from '@/types/IBearer';

import CustomInput from '../components/CustomInput.vue';
import ErrorFields from '../components/ErrorFields.vue';
import { useWorkspaces } from '@/composables/state/globalState';

export default defineComponent({
    components: {
        CustomInput,
        ErrorFields
    },
    setup() {
        const { fetchWorkspaces } = useWorkspaces();
        const username: Ref<string> = ref('');
        const password: Ref<string> = ref('');
        const errors: Ref<string[]> = ref([]);
        const router: Router = useRouter();

        const updateUsername = (userInput: string) => {
            username.value = userInput;
        }

        const updatePassword = (passwordInput: string) => {
            password.value = passwordInput;
        }

        const submit = async () => {
            const loginResult: IResult<IBearer> = await signin(username.value, password.value);

            if (loginResult.success && loginResult.data?.token) {
                const [ _, setSignedIn ] = useAuthState();
                setJWT(loginResult.data.token);
                // TODO(n): put this somewhere else
                window.localStorage.setItem('username', username.value);
                setSignedIn(username.value);
                await fetchWorkspaces();
                router.push('/');
            } else {
                errors.value = [...new Set(loginResult.errors)];
            }
        }
        return {
            updateUsername,
            updatePassword,
            submit,
            errors
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
