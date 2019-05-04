import AttachmentService from "./AttachmentService";
import { apiFetchTyped, getGeneralError, uploadEach } from "./Helpers";
import { IApiResponse } from "./interfaces/Common";
import {
    ICreateFaultForm,
    IFaultCreateResponse,
    IFaultDetails,
    IShortFaultDetails,
} from "./interfaces/FaultInterfaces";
import { IInvoiceDetails } from "./interfaces/InvoiceInterfaces";

class IncidentService {
    public static async getIncident(agreementId: string, incidentId: string): Promise<IApiResponse<IFaultDetails>> {
        try {
            const [, parsed] = await apiFetchTyped<IFaultDetails>(
                `/api/agreement/${agreementId}/fault/${incidentId}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IFaultDetails>();
        }
    }

    public static async getIncidents(agreementId: string, offset: number): Promise<IApiResponse<IShortFaultDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IShortFaultDetails[]>(
                `/api/agreement/${agreementId}/fault?offset=${offset}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IShortFaultDetails[]>();
        }
    }

    public static async updateIncident(
        agreementId: string,
        incidentId: string,
        price: number
    ): Promise<IApiResponse<any>> {
        try {
            const [, parsed] = await apiFetchTyped<any>(
                `/api/agreement/${agreementId}/fault/${incidentId}/fixed`,
                {
                    body: JSON.stringify({ price }),
                    method: "POST",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<any>();
        }
    }

    public static async deleteIncident(agreementId: string, incidentId: string): Promise<IApiResponse<any>> {
        try {
            const [, parsed] = await apiFetchTyped<any>(
                `/api/agreement/${agreementId}/fault/${incidentId}`,
                {
                    method: "DELETE",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<any>();
        }
    }

    public static async createIncident(
        agreementId: string,
        incident: ICreateFaultForm,
        files: File[]
    ): Promise<IApiResponse<IFaultCreateResponse>> {
        try {
            incident.attachments = files.map((f) => ({ name: f.name }));
            const [, parsed] = await apiFetchTyped<IFaultCreateResponse>(
                `/api/agreement/${agreementId}/fault`,
                { method: "POST", body: JSON.stringify(incident) },
                true
            );

            if (parsed.data !== undefined && files.length > 0) {
                const errors = await uploadEach(parsed.data.attachments, files, AttachmentService.putAttachment);
                parsed.errors = errors;
            }

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IFaultCreateResponse>();
        }
    }
}

export default IncidentService;
