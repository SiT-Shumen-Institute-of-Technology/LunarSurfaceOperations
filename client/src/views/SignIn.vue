<template>
    <form @keypress.enter.prevent="submit">
        <div class="form singin">
            <div v-if="errors.length > 0" class="form__field form__field--errors">
                <ErrorFields :errors="errors" />
            </div>

            <div class="form__field">
                <CustomInput type="text" v-model:input="username" :maxWidth="true" :opaque="true" placeholder="username"/>                
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
import { defineComponent, Ref, ref } from 'vue'
import { Router, useRouter } from 'vue-router';

import { useAuthState, setJWT } from '@/composables/state/globalState';
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

        const submit = async () => {
            const loginResult: IResult<IBearer> = await signin(username.value, password.value);

            if (loginResult.success && loginResult.data?.token) {
                const { setSignedIn } = useAuthState();
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
            username,
            password,
            submit,
            errors
        }
    }
})
</script>
