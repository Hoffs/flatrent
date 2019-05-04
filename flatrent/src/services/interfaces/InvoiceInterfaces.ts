import { IShortFaultDetails } from "./FaultInterfaces";

export interface IInvoiceDetails {
    id: string;
    amountToPay: number;
    dueDate: string;
    paidDate: string | null;
    invoicedPeriodFrom: string;
    invoicedPeriodTo: string;
    isPaid: boolean;
    isValid: boolean;
    isOverdue: boolean;
    faults: IShortFaultDetails[];
}
