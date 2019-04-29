import { apiFetch, apiFetchTyped, getGeneralError } from "./Helpers";
import { IBasicResponse, IErrorResponse } from "./interfaces/Common";
import UserService from "./UserService";
import { saveAs } from 'file-saver';
import { toast } from "react-toastify";

class AttachmentService {
  public static async putAttachment(fileId: string, file: File): Promise<IBasicResponse> {
    const data: IBasicResponse = {};
    try {
      const formData = new FormData();
      formData.append("file", file, file.name);

      const result = await fetch(`/api/attachment/${fileId}`, {
        body: formData,
        headers: UserService.authorizationHeaders(),
        method: "PUT",
      });

      if (result.ok) {
        console.log("uploaded image");
      } else {
        console.log("didnt upload flat");
        const response = (await result.json()) as IErrorResponse;
        data.errors = response;
      }
    } catch (e) {
      console.log(e);
      return getGeneralError<any>();
    }
    return data;
  }

  public static async downloadAttachment(id: string, name: string): Promise<void> {
    try {
      const result = await apiFetch(`/api/attachment/${id}`, undefined, true);
      saveAs(await result.blob(), name);
    } catch (e) {
      console.log(e);
      toast.error("Įvyko nežinoma klaida");
    }
    return;
  }
}

export default AttachmentService;
