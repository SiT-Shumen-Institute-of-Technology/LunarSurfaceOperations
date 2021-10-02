export interface IAttribute {
    type: string,
    attibuteName: string,
    value: string
}

export interface IMessage {
    id?: string,
    text: string;
    attributes?: IAttribute[],
    isMe?: boolean
}
