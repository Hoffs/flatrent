import { IShortFlatDetails } from "./FlatServiceInterfaces";
import { IShortUserDetails } from "./UserInterfaces";
import { IConversationDetails } from "./ConversationInterfaces";
import { IAttachment, IFileMetadata, IFileResponse } from "./Common";

export interface IShortIncidentDetails {
    id: string;
    name: string;
    repaired: boolean;
    price: number;
}

export interface IIncidentDetails {
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

export interface ICreateIncidentForm {
    name: string;
    description: string;
    attachments: IFileMetadata[];
}

export interface IIncidentCreateResponse {
    id: string;
    attachments: IFileResponse;
}
