export interface IAttribute {
    type: string,
    attributeName?: string,
    value?: string
}

export interface IMessage {
    id?: string,
    text: string;
    attributes?: IAttribute[],
    isMe?: boolean
}

export interface IUseMessages {
    currentConnectionMessages: any,
    addMessage: (message: IMessage) => void,
    setMessages: (messages: IMessage[]) => void,
    updateMessage: (message: IMessage) => void
}
