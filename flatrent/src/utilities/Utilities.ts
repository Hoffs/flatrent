export const joined = (...classNames: Array<string | undefined>): string => {
    return classNames.filter((cn) => cn !== undefined).join(" ");
};

export const dayOrDays = (days: number): string => days.toString(10).slice(-1) === "1" ? "diena" : "dienos";
export const roomOrRooms = (roomCount: number): string => roomCount > 1 ? "kambariai" : "kambarys";

export const userProfileUrl = (id: string): string => `/user/${id}`;
export const conversationWithUserUrl = (id: string): string => `/conversation/new?recipientId=${id}`;
export const flatRentUrl = (id: string): string => `/flat/${id}/rent`;
export const flatUrl = (id: string): string => `/flat/${id}`;

export const stopPropogation = (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => event.stopPropagation();
