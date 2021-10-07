<template>
    <form @keypress.enter.prevent="submit">
        <div class="form">
            <div v-if="errors.length > 0" class="form__field form__field--errors">
                <ErrorFields :errors="errors" />
            </div>

            <div class="form__field">
                <CustomInput type="text" v-model:input="name" :maxWidth="true" :opaque="true" placeholder="workspace name"/>                
            </div>

            <div class="form__field">
            </div>

            <div class="form__field">
                <CustomInput type="text" v-model:input="description" :maxWidth="true" :opaque="true" placeholder="real short description"/>                
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
import { useWorkspaces } from '@/composables/state/globalState';
import { createWorkspace } from '@/services/API/workspaces';
import { IVoidResult } from '@/types/IResult';
import { defineComponent, Ref, ref } from 'vue'
import { useRouter } from 'vue-router';

import CustomInput from '../components/CustomInput.vue';
import ErrorFields from '../components/ErrorFields.vue';

export default defineComponent({
    components: {
        CustomInput,
        ErrorFields
    },
    setup() {
        const { fetchWorkspaces } = useWorkspaces();

        const router = useRouter()
        const errors: Ref<string[]> = ref([]);
        const name: Ref<string> = ref('');
        const description: Ref<string> = ref('');

        const submit = async () => {
            const result: IVoidResult = await createWorkspace(name.value, description.value);

            if (result.success) {
                await fetchWorkspaces();
                router.push('/');
            } else {
                errors.value = [...new Set(result.errors)];
            }
        }

        return {
            name,
            description,
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
