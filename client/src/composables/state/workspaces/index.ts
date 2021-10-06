import {readonly, ref, Ref} from "vue";
import {IResult} from "@/types/IResult";
import {useAuthState} from "@/utils/globalUtils";
import {getWorkspaces} from "@/services/API/workspaces";
import {IWorkspace, IUseWorkspaces} from "@/types/IWorkspace";

const workspaces: Ref<IWorkspace[]> = ref([]);

export function useWorkspaces(): IUseWorkspaces  {
    const setWorkspaces = (workspacesInput: IWorkspace[]) => {
        workspaces.value = workspacesInput;
    };

    const addWorkspace = (workspace: IWorkspace): void => {
        workspaces.value.push(workspace);
    };
        
    const fetchWorkspaces = async (): Promise<void> => {
        const [isSignedUp] = useAuthState();

        if (isSignedUp) {
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
        
        fetchWorkspaces: fetchWorkspaces
    }
}
