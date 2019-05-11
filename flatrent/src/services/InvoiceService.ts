import { saveAs } from "file-saver";
import { apiFetch, apiFetchTyped, getGeneralError } from "./Helpers";
import { IApiResponse, IBasicResponse } from "./interfaces/Common";
import { IInvoiceDetails } from "./interfaces/InvoiceInterfaces";
import UserService from "./UserService";

class InvoiceService {
    public static getPdf = async (agreementId: string, invoiceId: string): Promise<IBasicResponse> => {
        try {
            const result = await apiFetch(`/api/agreement/${agreementId}/invoice/${invoiceId}/pdf`, {
                headers: UserService.authorizationHeaders(),
            });
            if (result.ok) {
                saveAs(await result.blob(), `invoice-${invoiceId}.pdf`);
                return {};
            } else {
                console.log("didnt get pdf");
                const response = (await result.json()) as IBasicResponse;
                if (response.message !== undefined && response.errors === undefined) {
                    response.errors = { general: [response.message] };
                }
                return response;
            }
        } catch (e) {
            console.log(e);
            return getGeneralError();
        }
    };

    public static async getInvoices(agreementId: string, offset: number): Promise<IApiResponse<IInvoiceDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IInvoiceDetails[]>(
                `/api/agreement/${agreementId}/invoice?offset=${offset}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IInvoiceDetails[]>();
        }
    }

    public static async payInvoice(agreementId: string, invoiceId: string): Promise<IApiResponse<any>> {
        try {
            const [, parsed] = await apiFetchTyped<any>(
                `/api/agreement/${agreementId}/invoice/${invoiceId}/pay`,
                {
                    method: "POST",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IInvoiceDetails[]>();
        }
    }
}

export default InvoiceService;
