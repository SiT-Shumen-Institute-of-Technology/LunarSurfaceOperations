<template>
    <div class="register">
        <div class="register-wrapper">
            <div class="error-wrapper" v-for="error in errors" :key="error">
                <div class="error"> {{ error }} </div>
            </div>
            <CustomInput type="text" label="Username" @update-value="updateUsername" />

            <CustomInput type="email" label="Email" @update-value="updateEmail"/>

            <CustomInput type="password" label="Password" @update-value="updatePassword" />

            <button @click="submit">Submit</button>
        </div>
    </div>
</template>

<script lang="ts">
import { register } from '@/services/auth';
import { IVoidResult } from '@/types/IResult';
import { defineComponent, Ref, ref } from 'vue'
import { Router, useRouter } from 'vue-router';

import CustomInput from '../components/CustomInput.vue';

export default defineComponent({
    components: {
        CustomInput
    },
    setup() {
        const username: Ref<string> = ref('');
        const email: Ref<string> = ref('');
        const password: Ref<string> = ref('');
        const errors: Ref<string[]> = ref([]);
        const router: Router = useRouter();

        const updateUsername = (userInput: string) => {
            username.value = userInput;
        }

        const updatePassword = (passwordInput: string) => {
            password.value = passwordInput;
        }

        const updateEmail = (emailInput: string) => {
            email.value = emailInput;
        }

        const submit = async () => {
            const registerResult: IVoidResult = await register(username.value, email.value, password.value);

            if (registerResult.success) {
                router.push('/signin');
            } else {
                errors.value = registerResult.errors;
            }
        }
        return {
            updateUsername,
            updatePassword,
            updateEmail,
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
