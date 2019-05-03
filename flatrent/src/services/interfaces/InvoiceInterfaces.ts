import { IShortFaultDetails } from "./FaultInterfaces";

export interface IInvoiceDetails {
    amountToPay: number;
    dueDate: string;
    paidDate: string | null;
    invoicePeriodFrom: string;
    invoicePeriodTo: string;
    isPaid: boolean;
    isValid: boolean;
    isOverdue: boolean;
    fauls: IShortFaultDetails[];
}
