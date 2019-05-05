import { IAttachment, IFileResponse } from "./Common";
import { IShortUserDetails } from "./UserInterfaces";

export interface IConversationDetails {
    id: string;
    subject: string;
    recipient: IShortUserDetails;
    author: IShortUserDetails;
}

export interface IMessageDetails {
    id: string;
    content: string;
    attachments: IAttachment[];
    authorId: string;
    createdDate: string;
}

export interface ICreatedMessageResponse {
    id: string;
    attachments: IFileResponse;
}

export interface ICreatedConversationResponse {
    id: string;
}
