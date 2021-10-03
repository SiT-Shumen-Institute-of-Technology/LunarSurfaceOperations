export interface IAttribute {
    type: string,
}

export interface IMessage {
    id?: string,
    text: string;
    attributes?: IAttribute[],
    isMe?: boolean
}
