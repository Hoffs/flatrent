import { IShortAddress, IShortFlatDetails } from "./FlatServiceInterfaces";
import { IShortUserDetails } from "./UserInterfaces";
import { IAttachment } from "./Common";
import { IConversationDetails } from "./ConversationInterfaces";

export enum AgreementStatusesText {
    Requested = "Išsiųsta nuomotojui",
    Accepted = "Patvirtinta",
    Rejected = "Atmesta",
    Expired = "Nepatvirtinta laiku",
    Ended = "Pasibaigusi",
}

export enum AgreementStatuses {
    Requested = 1,
    Accepted = 2,
    Rejected = 3,
    Expired = 4,
    Ended = 5,
}

export const getAgreementStatusText = (id: number): string => {
    if (id === AgreementStatuses.Requested) {
        return AgreementStatusesText.Requested;
    }
    if (id === AgreementStatuses.Accepted) {
        return AgreementStatusesText.Accepted;
    }
    if (id === AgreementStatuses.Rejected) {
        return AgreementStatusesText.Rejected;
    }
    if (id === AgreementStatuses.Expired) {
        return AgreementStatusesText.Expired;
    }
    if (id === AgreementStatuses.Ended) {
        return AgreementStatusesText.Ended;
    }
    return "Nežinoma";
};

export interface IShortAgreementDetails {
    id: string;
    from: string;
    to: string;
    flatName: string;
    address: IShortAddress;
    status: IAgreementStatus;
}

export interface IAgreementStatus {
    id: number;
    name: string;
}

export interface IAgreementShortUserDetails extends IShortUserDetails {
    email: string;
    phoneNumber: string;
}

export interface IAgreementDetails {
    id: string;
    status: IAgreementStatus;
    from: string;
    to: string;
    comments: string;
    price: number;
    flat: IShortFlatDetails;
    tenant: IAgreementShortUserDetails;
    owner: IAgreementShortUserDetails;
    attachments: IAttachment[];
    conversation: IConversationDetails;
}
