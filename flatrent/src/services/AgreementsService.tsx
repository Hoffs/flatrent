import { saveAs } from "file-saver";
import { apiFetch, apiFetchTyped, getGeneralError } from "./Helpers";
import { IAgreementDetails } from "./interfaces/AgreementInterfaces";
import { IApiResponse, IBasicResponse, IErrorResponse } from "./interfaces/Common";
import UserService from "./UserService";

class AgreementsService {
  public static getDetails = async (id: string): Promise<IApiResponse<IAgreementDetails>> => {
    try {
      const [, parsed] = await apiFetchTyped<IAgreementDetails>(`/api/agreement/${id}`, undefined, true);
      return parsed;
    } catch (e) {
      console.log(e);
      return getGeneralError<IAgreementDetails>();
    }
  };

  public static getPdf = async (id: string): Promise<IErrorResponse> => {
    try {
      const result = await apiFetch(`/api/agreement/${id}/pdf`, {
        headers: UserService.authorizationHeaders(),
      });
      if (result.ok) {
        saveAs(await result.blob(), `agreement-${id}.pdf`);
        return {};
      } else {
        console.log("didnt get pdf");
        const response = (await result.json()) as IErrorResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return { General: ["Įvyko nežinoma klaida"] };
    }
  };

  public static acceptAgreement = async (id: string): Promise<IBasicResponse> => {
    try {
      const result = await apiFetch(
        `/api/agreement/${id}/accept`,
        {
          method: "POST",
        },
        true
      );
      if (result.ok) {
        return {};
      } else {
        const response = (await result.json()) as IBasicResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return getGeneralError<any>();
    }
  };

  public static rejectAgreement = async (id: string): Promise<IBasicResponse> => {
    try {
      const result = await apiFetch(
        `/api/agreement/${id}/reject`,
        {
          method: "POST",
        },
        true
      );
      if (result.ok) {
        return {};
      } else {
        const response = (await result.json()) as IBasicResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return getGeneralError<any>();
    }
  };

  public static cancelAgreement = async (id: string): Promise<IBasicResponse> => {
    try {
      const result = await apiFetch(
        `/api/agreement/${id}`,
        {
          method: "DELETE",
        },
        true
      );
      if (result.ok) {
        return {};
      } else {
        console.log("didnt delete");
        const response = (await result.json()) as IBasicResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return getGeneralError<any>();
    }
  };
}

export default AgreementsService;
