import {readonly, ref, Ref} from "vue";
import {IMessage, IUseMessages} from "@/types/IMessage";

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
