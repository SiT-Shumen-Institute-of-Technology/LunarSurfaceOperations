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
import { createWorkspace } from '@/services/workspaces';
import { IVoidResult } from '@/types/IResult';
import { defineComponent, Ref, ref } from 'vue'
import { useStore } from 'vuex';

import CustomInput from '../components/CustomInput.vue';
import ErrorFields from '../components/ErrorFields.vue';

export default defineComponent({
    components: {
        CustomInput,
        ErrorFields
    },
    setup() {
        const store = useStore();
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
                store.dispatch('fetchWorkspaces');
            } else {
                errors.value = [...new Set(result.errors)];
            }
        }

        return {
            updateDescription,
            updateName,
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
