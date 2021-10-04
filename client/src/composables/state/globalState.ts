import {readonly, ref, Ref} from "vue";
import {IResult} from "@/types/IResult";
import {useAuthState} from "@/utils/globalUtils";
import {getWorkspaces} from "@/services/workspaces";
import {IMessage, IUseMessages} from "@/types/IMessage";
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

            if (fetchWorkspaces.success) {
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

const currentConnectionMessages: Ref<IMessage[]> = ref([]);

export function useCurrentWorkspaceMessages(): IUseMessages {
    return {
        currentConnectionMessages: readonly(currentConnectionMessages),

        addMessage: (message: IMessage): void => {
            currentConnectionMessages.value.push(message);
        },

        setMessages: (messages: IMessage[]) => {
            currentConnectionMessages.value = messages;
        },
        
        updateMessage: (message: IMessage) => {
            const index = currentConnectionMessages.value.findIndex(el => {
                return el.id === message.id
            });

            currentConnectionMessages.value[index] = message;
        }
    }
}
