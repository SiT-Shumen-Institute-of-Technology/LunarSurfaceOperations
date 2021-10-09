<template>
    <div class="workspace" v-for="workspace in workspaces" :key="workspace">
        <router-link class="link workspace__link" :to="{ name: 'Workspace', params: { id: workspace.id } }">
            {{ workspace.name }}
        </router-link>
        <button v-if="currentlyLoggedUsername === workspace.owner.username"
            @click="toggleUpdateModal(workspace.id)" 
            class="workspace__update-users"> + </button>

        <Modal v-if="showUpdateModal"
            titleText="Update workspace"
            @modalClose="toggleUpdateModal"
        > 
            <div class="modal-content__body">
                <div class="modal-content__body__list">
                    <div class="list__header">
                        Users in this workspace
                    </div>
                    <div class="list__body">
                        <span v-for="user in users" :key="user.id" class="user">
                            {{ user.username }}
                        </span>
                    </div>
                </div>

                <div class="modal-content__body__disclamer">
                    <em>Put the usernames of the users in the input field bellow separated with comma and space after each username</em>
                    <br>
                    <em>For example: Nazgo, Tony</em>
                </div>

                <div class="modal-content__body__input">
                    <CustomInput 
                        :maxWidth="true"
                        :opaque="false"
                        placeholder="list of users"
                        type="text"
                        v-model:input="updateUsersInput"
                        :alwaysActiveLabel="true"
                    />
                </div>

                <div class="modal-content__body__buttons">
                    <button @click="update('add', workspace.id)"
                        class="button button-add">Add</button>
                    <button @click="update('remove', workspace.id)" 
                        class="button button--remove">Remove</button>
                </div>
            </div>
        </Modal>
    </div>
</template>

<script lang="ts">
import { defineComponent, Ref, ref } from 'vue'

import Modal from '@/components/Modal.vue';
import CustomInput from '@/components/CustomInput.vue';

import { IUser } from '@/types/IUser';
import { IResult } from '@/types/IResult';
import { getMembers, putMemebers } from '@/services/API/workspaces';
import { useAuthState } from '@/composables/state/auth';

export default defineComponent({
    components: {
        Modal,
        CustomInput
    },
    props: {
        workspaces: Array 
    },
    setup() {
        const UPDATE_MODE_ADD = 'add';
        const UPDATE_MODE_REMOVE = 'remove';

        const users: Ref<IUser[]> = ref([]);
        const errors: Ref<string[]> = ref([]);
        const showUpdateModal: Ref<boolean> = ref(false);
        const updateUsersInput: Ref<string> = ref('');

        const { currentlyLoggedUsername } = useAuthState();

        const update = async (mode: string, workspaceId: string) => {
            if (updateUsersInput.value.length > 0) {
                let usernames = users.value.map(user => user.username);
                const usernamesToModify = updateUsersInput.value.split(', ');

                if (mode === UPDATE_MODE_ADD) {
                    usernames.push(...usernamesToModify);
                } else if (mode === UPDATE_MODE_REMOVE) {
                    usernames = usernames.filter(username => !usernamesToModify.includes(username));
                }

                const result = await putMemebers(workspaceId, usernames);
                console.log(result);
            
                if (result.success) {
                    const newUsersObjects: IUser[] = [];

                    usernames.forEach((username, i) => {
                        newUsersObjects.push({
                            username,
                            id: ""+i 
                        });
                    });

                    users.value = newUsersObjects;
                    updateUsersInput.value = '';
                }
            }
        }

        const toggleUpdateModal = async (id: string) => {
            showUpdateModal.value = !showUpdateModal.value;

            if (showUpdateModal.value && users.value.length <= 0 && id) {
                const result: IResult<IUser[]> = await getMembers(id);

                if (result.success && result.data) {
                    users.value = result.data;
                } else {
                    errors.value = result.errors
                }
            }
        }

        return {
            users,
            update,
            showUpdateModal,
            toggleUpdateModal,
            updateUsersInput,
            currentlyLoggedUsername
        }
    }
})
</script>

<style lang="less" scoped>
.workspace {
    display: grid;
    grid-template-columns: repeat(4, 1fr);

    &__link {
        display: block;
        grid-column-end: span 3;
    }

    &__update-users {
        grid-column-end: span 1;
        padding: 20px;
        text-align: center;

        background-color: inherit;
        border: 0;
        outline: 0;

        &:hover {
            cursor: pointer;
            background-color: lightblue;
        }
    }
}

.modal-content__body {
    padding: 15px;

    & > *:not(:first-child) {
        padding-top: 5px;
    }

    &__list {
        background-color: lightgray;
        box-sizing: border-box;
        border: 1px solid black;

        .padding {
            padding: 10px;    
        }

        .list__header {
            .padding;
            text-align: center;
            border-bottom: 1px solid black;

        }

        .list__body {
            .padding;
            .user {
                &:not(:last-child):after {
                    content: ', ';
                }
            } 
        }

    }

    &__disclamer {
        font-size: 11px;
        color: darkgray;
    }

    &__buttons {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        grid-column-gap: 5px;

        .button {
            border: 0;
            text-transform: uppercase;
            background-color: black;
            color: white;
            padding: 10px 0;

            opacity: 0.9;

            &:hover {
                cursor: pointer;
            }
        }
    }
}
</style>
