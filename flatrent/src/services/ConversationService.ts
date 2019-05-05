import { toast } from "react-toastify";
import AttachmentService from "./AttachmentService";
import { apiFetchTyped, getGeneralError, uploadEach } from "./Helpers";
import { IApiResponse } from "./interfaces/Common";
import {
    IConversationDetails,
    ICreatedMessageResponse,
    IMessageDetails,
    ICreatedConversationResponse,
} from "./interfaces/ConversationInterfaces";

class ConversationService {
    public static async getConversations(offset: number): Promise<IApiResponse<IConversationDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IConversationDetails[]>(
                `/api/conversation?offset=${offset}`,
                {
                    method: "GET",
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<IConversationDetails[]>();
        }
    }

    public static async newConversation(recipient: string): Promise<IApiResponse<ICreatedConversationResponse>> {
        try {
            const [, parsed] = await apiFetchTyped<ICreatedConversationResponse>(
                `/api/conversation`,
                {
                    method: "POST",
                    body: JSON.stringify({ recipientId: recipient }),
                },
                true
            );

            return parsed;
        } catch (e) {
            console.log(e);
            return getGeneralError<ICreatedConversationResponse>();
        }
    }

    public static async getConversation(conversationId: string): Promise<IApiResponse<IConversationDetails>> {
        try {
            const [, parsed] = await apiFetchTyped<IConversationDetails>(
                `/api/conversation/${conversationId}`,
                undefined,
                true
            );
            return parsed;
        } catch (e) {
            console.log(e);
            toast.error("Įvyko nežinoma klaida");
        }
        return getGeneralError<IConversationDetails>();
    }

    public static async getMessages(conversationId: string, offset: number): Promise<IApiResponse<IMessageDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IMessageDetails[]>(
                `/api/conversation/${conversationId}/messages?offset=${offset}`,
                undefined,
                true
            );
            return parsed;
        } catch (e) {
            console.log(e);
            toast.error("Įvyko nežinoma klaida");
        }
        return getGeneralError<IMessageDetails[]>();
    }

    public static async getNewMessages(
        conversationId: string,
        lastMessageId: string
    ): Promise<IApiResponse<IMessageDetails[]>> {
        try {
            const [, parsed] = await apiFetchTyped<IMessageDetails[]>(
                `/api/conversation/${conversationId}/new?lastMessageId=${lastMessageId}`,
                undefined,
                true
            );
            return parsed;
        } catch (e) {
            console.log(e);
            toast.error("Įvyko nežinoma klaida");
        }
        return getGeneralError<IMessageDetails[]>();
    }

    public static async createMessage(
        conversationId: string,
        message: string,
        files: File[]
    ): Promise<IApiResponse<ICreatedMessageResponse>> {
        try {
            const [, parsed] = await apiFetchTyped<ICreatedMessageResponse>(
                `/api/conversation/${conversationId}`,
                {
                    body: JSON.stringify({ content: message, attachments: files.map((f) => ({ name: f.name })) }),
                    method: "POST",
                },
                true
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
