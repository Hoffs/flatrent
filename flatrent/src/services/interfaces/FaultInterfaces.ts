import { IShortFlatDetails } from "./FlatServiceInterfaces";
import { IShortUserDetails } from "./UserInterfaces";
import { IConversationDetails } from "./ConversationInterfaces";
import { IAttachment, IFileMetadata, IFileResponse } from "./Common";

export interface IShortFaultDetails {
    id: string;
    name: string;
    repaired: boolean;
    price: number;
}

export interface IFaultDetails {
    id: string;
    name: string;
    description: string;
    repaired: boolean;
    price: number;

    flat: IShortFlatDetails;
    tenant: IShortUserDetails;
    owner: IShortUserDetails;
    conversation: IConversationDetails;
    attachments: IAttachment[];
}

export interface ICreateFaultForm {
    name: string;
    description: string;
    attachments: IFileMetadata[];
}

export interface IFaultCreateResponse {
    id: string;
    attachments: IFileResponse;
}
