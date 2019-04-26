import { apiFetch } from "./Helpers";
import { IBasicResponse, IErrorResponse } from "./interfaces/Common";
import UserService from "./UserService";

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
      data.errors = { General: ["Įvyko nežinoma klaida"] };
    }
    return data;
  }
}

export default AttachmentService;
