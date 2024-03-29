export const joined = (...classNames: Array<string | undefined>): string => {
    return classNames.filter((cn) => cn !== undefined).join(" ");
};

export const dayOrDays = (days: number): string => (days.toString(10).slice(-1) === "1" ? "diena" : "dienos");
export const roomOrRooms = (roomCount: number): string => (roomCount > 1 ? "kambariai" : "kambarys");

export const userProfileUrl = (id: string): string => `/user/${id}`;
export const userProfileEditUrl = (id: string): string => `/user/${id}/edit`;
export const conversationUrl = (id: string): string => `/conversations/${id}`;
export const conversationWithUserUrl = (id: string): string => `/conversations/new?recipientId=${id}`;
export const flatEditUrl = (id: string): string => `/flat/${id}/edit`;
export const flatRentUrl = (id: string): string => `/flat/${id}/rent`;
export const agreementUrl = (id: string): string => `/agreement/${id}`;
export const invoiceUrl = (agreementId: string, id: string): string => `${agreementUrl(agreementId)}/invoice/${id}`;
export const incidentUrl = (agreementId: string, id: string): string => `${agreementUrl(agreementId)}/incident/${id}`;
export const incidentCreateUrl = (agreementId: string): string => `${agreementUrl(agreementId)}/incident/new`;

export const flatUrl = (id: string): string => `/flat/${id}`;
export const loginUrl = (): string => `/login`;

export const stopPropogation = (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => event.stopPropagation();

export function uncapitalize(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}
