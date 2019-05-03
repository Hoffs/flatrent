import { toast } from "react-toastify";
import AttachmentService from "./AttachmentService";
import { apiFetch, apiFetchTyped, getGeneralError, uploadEach } from "./Helpers";
import { IApiResponse, IBasicResponse, IErrorResponse } from "./interfaces/Common";
import { IConversationDetails, ICreatedMessageResponse, IMessageDetails } from "./interfaces/ConversationInterfaces";
import UserService from "./UserService";

class ConversationService {
  public static async getConversation(conversationId: string): Promise<IApiResponse<IConversationDetails>> {
    try {
      const [result, parsed] = await apiFetchTyped<IConversationDetails>(
        `/api/conversation/${conversationId}`,
        undefined,
        true);
      return parsed;
    } catch (e) {
      console.log(e);
      toast.error("Įvyko nežinoma klaida");
    }
    return getGeneralError<IConversationDetails>();
  }

  public static async getMessages(conversationId: string, offset: number): Promise<IApiResponse<IMessageDetails[]>> {
    try {
      const [result, parsed] = await apiFetchTyped<IMessageDetails[]>(
        `/api/conversation/${conversationId}/messages?offset=${offset}`,
        undefined,
        true);
      return parsed;
    } catch (e) {
      console.log(e);
      toast.error("Įvyko nežinoma klaida");
    }
    return getGeneralError<IMessageDetails[]>();
  }

  public static async createMessage(conversationId: string, message: string, files: File[]):
      Promise<IApiResponse<ICreatedMessageResponse>> {
    try {
      const [result, parsed] = await apiFetchTyped<ICreatedMessageResponse>(
        `/api/conversation/${conversationId}`,
        {
          body: JSON.stringify({ content: message, attachments: files.map((f) => ({ name: f.name })) }),
          method: "POST",
        },
        true,
      );
      if (parsed.data !== undefined) {
        const errors = await uploadEach(parsed.data.attachments, files, AttachmentService.putAttachment);
        if (errors !== undefined) {
          return { errors };
        }
      }
      return parsed;
    } catch (e) {
      console.log(e);
      toast.error("Įvyko nežinoma klaida");
    }
    return getGeneralError<ICreatedMessageResponse>();
  }
}

export default ConversationService;