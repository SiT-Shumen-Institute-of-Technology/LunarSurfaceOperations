import { createStore } from 'vuex'

import { IResult } from '@/types/IResult';
import { getWorkspaces } from '@/services/workspaces'
import { IWorkspace } from '@/types/IWorkspace'
import {useAuthState} from '@/utils/globalUtils';

export default createStore({
  state: {
      workspaces: Array<IWorkspace>()
  },
  mutations: {
    setWorkspaces(state, data: IWorkspace[]) {
        state.workspaces = data;
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
