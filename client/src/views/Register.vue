<template>
    <form @keydown.enter.prevent="submit">
        <div class="form">
            <div v-if="errors.length > 0" class="form__field form__field--errors">
                <ErrorFields :errors="errors" />
            </div>

            <div class="form__field">
                <CustomInput type="text" v-model:input="username" :maxWidth="true" :opaque="true" placeholder="username"/>                
            </div>

            <div class="form__field">
            </div>

            <div class="form__field">
                <CustomInput type="email" v-model:input="email" :maxWidth="true" :opaque="true" placeholder="email"/>                
            </div>

            <div class="form__field">
            </div>

            <div class="form__field">
                <CustomInput type="password" v-model:input="password" :maxWidth="true" :opaque="true" placeholder="password"/>                
            </div>

            <div class="form__field">
            </div>

            <button class="form__submit" @click="submit">
                Submit
            </button>
        </div>
    </form>
</template>

<script lang="ts">
import { register } from '@/services/API/auth';
import { IVoidResult } from '@/types/IResult';
import { defineComponent, Ref, ref } from 'vue'
import { Router, useRouter } from 'vue-router';

import CustomInput from '../components/CustomInput.vue';
import ErrorFields from '../components/ErrorFields.vue';

export default defineComponent({
    components: {
        CustomInput,
        ErrorFields
    },
    setup() {
        const username: Ref<string> = ref('');
        const email: Ref<string> = ref('');
        const password: Ref<string> = ref('');
        const errors: Ref<string[]> = ref([]);
        const router: Router = useRouter();

        const submit = async () => {
            const registerResult: IVoidResult = await register(username.value, email.value, password.value);

            if (registerResult.success) {
                router.push('/signin');
            } else {
                errors.value = [...new Set(registerResult.errors)];
            }
        }
        return {
            username,
            password,
            email,
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
