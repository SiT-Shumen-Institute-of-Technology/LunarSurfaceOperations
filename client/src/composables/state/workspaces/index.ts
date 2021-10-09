import {readonly, ref, Ref} from "vue";
import {IResult} from "@/types/IResult";
import {getWorkspaces} from "@/services/API/workspaces";
import {IWorkspace, IUseWorkspaces} from "@/types/IWorkspace";
import {useAuthState} from '@/composables/state/globalState';

const workspaces: Ref<IWorkspace[]> = ref([]);

export function useWorkspaces(): IUseWorkspaces  {
    const setWorkspaces = (workspacesInput: IWorkspace[]) => {
        workspaces.value = workspacesInput;
    };

    const addWorkspace = (workspace: IWorkspace): void => {
        workspaces.value.push(workspace);
    };
        
    const fetchWorkspaces = async (): Promise<void> => {
        const { isSignedIn } = useAuthState();

        if (isSignedIn.value) {
            const fetchWorkspaces: IResult<IWorkspace[]> = await getWorkspaces();

            if (fetchWorkspaces.success && fetchWorkspaces.data) {
                setWorkspaces(fetchWorkspaces.data);
            }
        }
    };

    return {
        workspaces: readonly(workspaces),

        setWorkspaces: setWorkspaces,

        addWorkspace: addWorkspace,
        
        fetchWorkspaces: fetchWorkspaces,
    }
}
