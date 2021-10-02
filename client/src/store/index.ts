import { getWorkspaces } from '@/services/workspaces'
import { IResult } from '@/types/IResult';
import { IWorkspace } from '@/types/IWorkspace'
import { createStore } from 'vuex'
import { getWorkspaces } from '@/services/workspaces'
import { IResult } from '@/types/IResult';
import { IWorkspace } from '@/types/IWorkspace'

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
        const fetchWorkspaces: IResult<IWorkspace[]> = await getWorkspaces();
        commit('setWorkspaces', fetchWorkspaces.data);
    }
  },
  modules: {
  }
})
