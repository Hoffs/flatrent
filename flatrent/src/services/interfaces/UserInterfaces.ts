import { IShortAgreementData } from "./AgreementInterfaces";

export interface ILoginResponse {
    token: string;
}

export interface ITokenPayload {
    UserId: string;
    role: string | string[];
    exp: number;
}

export interface IRegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    phoneNumber: string;
    about: string;
    bankAccount: string;
}

export interface IUserDetails {
    id: string;
    firstName: string;
    lastName: string;
    phoneNumber: string;
    avatarId: string;
    about: string;
    tenantAgreementCount: number;
    ownerAgreementCount: number;
    createdDate: string;
}

export interface IShortUserDetails {
    id: string;
    firstName: string;
    lastName: string;
    avatarId: string;
}

export interface IClientData {
    id: string;
    description: string;
}

export interface IUserAgreements {
    ownerAgreements: IShortAgreementData[];
    tenantAgreements: IShortAgreementData[];
}
