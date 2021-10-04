<template>
    <div class="register">
        <div class="register-wrapper">
            <ErrorFields :errors="errors" />

            <CustomInput type="text" label="Name" @update-value="updateName" />

            <CustomInput type="text" label="Descriptions" @update-value="updateDescription" />

            <button @click="submit">Submit</button>
        </div>
    </div>
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

        const updateName = (nameInput: string) => {
            name.value = nameInput;
        }

        const updateDescription = (descriptionInput: string) => {
            description.value = descriptionInput;
        }

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
            updateDescription,
            updateName,
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
