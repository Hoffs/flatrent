import AttachmentService from "./AttachmentService";
import { apiFetchTyped, getGeneralError, uploadEach } from "./Helpers";
import { IApiResponse } from "./interfaces/Common";
import {
    ICreateIncidentForm,
    IIncidentCreateResponse,
    IIncidentDetails,
    IShortIncidentDetails,
} from "./interfaces/IncidentInterfaces";

class IncidentService {
    public static async getIncident(agreementId: string, incidentId: string): Promise<IApiResponse<IIncidentDetails>> {
        try {
            const [, parsed] = await apiFetchTyped<IIncidentDetails>(
                `/api/agreement/${agreementId}/incident/${incidentId}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IIncidentDetails>();
        }
    }

    public static async getIncidents(agreementId: string, offset: number): Promise<IApiResponse<IShortIncidentDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IShortIncidentDetails[]>(
                `/api/agreement/${agreementId}/incident?offset=${offset}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IShortIncidentDetails[]>();
        }
    }

    public static async updateIncident(
        agreementId: string,
        incidentId: string,
        price: number
    ): Promise<IApiResponse<any>> {
        try {
            const [, parsed] = await apiFetchTyped<any>(
                `/api/agreement/${agreementId}/incident/${incidentId}/fixed`,
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
                `/api/agreement/${agreementId}/incident/${incidentId}`,
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
        incident: ICreateIncidentForm,
        files: File[]
    ): Promise<IApiResponse<IIncidentCreateResponse>> {
        try {
            incident.attachments = files.map((f) => ({ name: f.name }));
            const [, parsed] = await apiFetchTyped<IIncidentCreateResponse>(
                `/api/agreement/${agreementId}/incident`,
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
            return getGeneralError<IIncidentCreateResponse>();
        }
    }
}

export default IncidentService;
