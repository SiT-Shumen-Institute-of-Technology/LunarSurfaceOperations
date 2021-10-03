import { createStore } from 'vuex'

import { IResult } from '@/types/IResult';
import { getWorkspaces } from '@/services/workspaces'
import { IWorkspace } from '@/types/IWorkspace'
import {useAuthState} from '@/utils/globalUtils';

export default createStore({
  state: {
      workspaces: Array<IWorkspace>(),
      currentConnectionMessages: Array<any>()
  },
  mutations: {
    setWorkspaces(state, data: IWorkspace[]) {
        state.workspaces = data;
    },

    setMessages(state, messages: any) {
        state.currentConnectionMessages = messages;
    },

    addMessage(state, message: any) {
        state.currentConnectionMessages.push(message);
    },

    updateMessage(state, message: any) {
        const index = state.currentConnectionMessages.findIndex(el => {
            return el.id === message.id
        });

        state.currentConnectionMessages[index] = message;
    }
  },
  actions: {
    async fetchWorkspaces({ commit }) {
        const [isSignedUp] = useAuthState();
        if (isSignedUp.value) {
            const fetchWorkspaces: IResult<IWorkspace[]> = await getWorkspaces();
            commit('setWorkspaces', fetchWorkspaces.data);
        }
    }
  },
  modules: {
  }
})
