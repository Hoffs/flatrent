import { IShortFlatDetails } from "./FlatServiceInterfaces";
import { IShortUserDetails } from "./UserInterfaces";
import { IConversationDetails } from "./ConversationInterfaces";
import { IAttachment } from "./Common";

export interface IShortFaultDetails {
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
