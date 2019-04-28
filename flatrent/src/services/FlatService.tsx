import { apiFetch, uploadEach, apiFetchTyped, getGeneralError } from "./Helpers";
import ImageService from "./ImageService";
import { IBasicResponse, IErrorResponse, IApiResponse } from "./interfaces/Common";
import {
  IFlatCreateRequest,
  IFlatCreateResponse,
  IFlatDetails,
  IShortFlatDetails,
  IFlatListResponse,
  IRentRequest,
  IAgreementCreateResponse,
  IAddress,
} from "./interfaces/FlatServiceInterfaces";
import UserService from "./UserService";
import AttachmentService from "./AttachmentService";
import { parseTwoDigitYear } from "moment";

export const getAddressString = (address: IAddress) => {
  console.log(address);
  return `${address.street} ${address.houseNumber}-${address.flatNumber}, ${address.city}, ${address.country}`;
};

class FlatService {
  public static async getFlats(count: number, offset: number): Promise<IFlatListResponse> {
    try {
      // const rentedQuery = rented ? "&rented=true" : "";
      const [result, parsed] = await apiFetchTyped<IShortFlatDetails[]>(
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
      const [response, parsed] = await apiFetchTyped<IFlatDetails>(`/api/flat/${id}`, {
        headers: UserService.authorizationHeaders(),
      });
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
      const [response, parsed] = await apiFetchTyped<IFlatCreateResponse>(`/api/flat/`, {
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
}

export default FlatService;
