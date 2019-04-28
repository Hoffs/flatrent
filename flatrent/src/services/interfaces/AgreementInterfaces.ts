import { IShortAddress } from "./FlatServiceInterfaces";
import { IShortUserDetails } from "./UserInterfaces";

export enum AgreementStatuses {
    Requested = "Išsiųsta",
    Accepted = "Patvirtinta",
    Rejected = "Atmesta",
    Expired = "Nepatvirtinta",
    Ended = "Pasibaigusi",
}

export const getAgreementStatusText = (id: number): string => {
    if (id === 1) { return AgreementStatuses.Requested; }
    if (id === 2) { return AgreementStatuses.Accepted; }
    if (id === 3) { return AgreementStatuses.Rejected; }
    if (id === 4) { return AgreementStatuses.Expired; }
    if (id === 5) { return AgreementStatuses.Ended; }
    return "Nežinoma";
}

export interface IShortAgreementData {
    id: string;
    from: string;
    to: string;
    flatName: string;
    address: IShortAddress;
    status: IAgreementStatus;
    tenant: IShortUserDetails;
    owner: IShortUserDetails;
}

export interface IAgreementStatus {
    id: number;
    name: string;
}
