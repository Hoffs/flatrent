import { saveAs } from "file-saver";
import { apiFetch } from "./Helpers";
import { IErrorResponse } from "./Settings";
import UserService from "./UserService";

class AgreementsService {
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

  public static cancelAgreement = async (id: string): Promise<IErrorResponse> => {
    try {
      const result = await apiFetch(`/api/agreement/${id}`, {
        headers: UserService.authorizationHeaders(),
        method: "DELETE",
      });
      if (result.ok) {
        return {};
      } else {
        console.log("didnt delete");
        const response = (await result.json()) as IErrorResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return { General: ["Įvyko nežinoma klaida"] };
    }
  };
}

export default AgreementsService;
