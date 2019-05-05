import AttachmentService from "./AttachmentService";
import { apiFetchTyped, getGeneralError, uploadEach } from "./Helpers";
import ImageService from "./ImageService";
import { IApiResponse } from "./interfaces/Common";
import {
    IAddress,
    IAgreementCreateResponse,
    IFlatCreateRequest,
    IFlatCreateResponse,
    IFlatDetails,
    IFlatListResponse,
    IRentRequest,
    IShortFlatDetails,
    IImageDetails,
} from "./interfaces/FlatServiceInterfaces";
import UserService from "./UserService";

export const getAddressString = (address: IAddress) => {
    console.log(address);
    return `${address.street} ${address.houseNumber}-${address.flatNumber}, ${address.city}, ${address.country}`;
};

class FlatService {
    public static async getFlats(count: number, offset: number): Promise<IFlatListResponse> {
        try {
            // const rentedQuery = rented ? "&rented=true" : "";
            const [, parsed] = await apiFetchTyped<IShortFlatDetails[]>(
                `/api/flat?count=${count}&offset=${offset}`,
                undefined,
                true
            );
            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IShortFlatDetails[]>();
        }
    }

    public static async getFlat(id: string): Promise<IApiResponse<IFlatDetails>> {
        try {
            const [, parsed] = await apiFetchTyped<IFlatDetails>(`/api/flat/${id}`, {
                headers: UserService.authorizationHeaders(),
            });
            return parsed;
        } catch (e) {
            console.log(e);
            return { errors: { General: ["Įvyko nežinoma klaida"] } };
        }
    }

    public static async deleteFlat(id: string): Promise<IApiResponse<any>> {
        try {
            const [, parsed] = await apiFetchTyped<any>(
                `/api/flat/${id}`,
                {
                    method: "DELETE",
                },
                true
            );
            return parsed;
        } catch (e) {
            console.log(e);
            return { errors: { General: ["Įvyko nežinoma klaida"] } };
        }
    }

    public static async rentFlat(
        id: string,
        request: IRentRequest,
        files: File[]
    ): Promise<IApiResponse<IAgreementCreateResponse>> {
        try {
            const [response, parsed] = await apiFetchTyped<IAgreementCreateResponse>(`/api/flat/${id}/rent`, {
                body: JSON.stringify({ ...request }),
                headers: UserService.authorizationHeaders(),
                method: "POST",
            });

            if (!response.ok) {
                console.log("didnt get flat");
                return parsed;
            }

            console.log("created request");
            if (parsed.data !== undefined) {
                const uploadResult = await uploadEach(parsed.data.attachments, files, AttachmentService.putAttachment);
                parsed.errors = uploadResult;
            }
            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IAgreementCreateResponse>();
        }
    }

    public static async createFlat(
        requestData: { [key: string]: string | boolean },
        images: File[]
    ): Promise<IApiResponse<IFlatCreateResponse>> {
        const request = (requestData as unknown) as IFlatCreateRequest;
        const featuresString = requestData.features as string;
        if (featuresString.split !== undefined) {
            request.features = (requestData.features as string).split(",");
        }
        request.images = images.map((i) => ({ name: i.name }));
        try {
            const [, parsed] = await apiFetchTyped<IFlatCreateResponse>(`/api/flat/`, {
                body: JSON.stringify(requestData),
                headers: UserService.authorizationHeaders(),
                method: "POST",
            });

            if (parsed.errors !== undefined) {
                return parsed;
            }

            if (parsed.data !== undefined) {
                const uploadResult = await uploadEach(parsed.data.images, images, ImageService.putFlatImage);
                parsed.errors = uploadResult;
            }
            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IFlatCreateResponse>();
        }
    }

    public static async updateFlat(
        id: string,
        requestData: { [key: string]: string | boolean },
        images: File[],
        oldImages: IImageDetails[],
    ): Promise<IApiResponse<IFlatCreateResponse>> {
        const request = (requestData as unknown) as IFlatCreateRequest;
        const featuresString = requestData.features as string;
        if (featuresString.split !== undefined) {
            request.features = (requestData.features as string).split(",");
        }
        request.images = images.map((i) => ({ name: i.name }));
        try {
            const [, parsed] = await apiFetchTyped<IFlatCreateResponse>(`/api/flat/${id}`, {
                body: JSON.stringify(requestData),
                headers: UserService.authorizationHeaders(),
                method: "PUT",
            });

            if (parsed.errors !== undefined) {
                return parsed;
            }

            if (parsed.data !== undefined) {
                const uploadResult = await uploadEach(parsed.data.images, images, ImageService.putFlatImage);
                parsed.errors = uploadResult;
            }
            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IFlatCreateResponse>();
        }
    }
}

export default FlatService;
