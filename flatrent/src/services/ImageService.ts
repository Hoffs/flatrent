import { IBasicResponse, IErrorResponse } from "./interfaces/Common";
import UserService from "./UserService";

class ImageService {
    public static async putFlatImage(imageId: string, file: File): Promise<IBasicResponse> {
        const data: IBasicResponse = {};
        try {
            const formData = new FormData();
            formData.append("image", file, file.name);

            const result = await fetch(`/api/image/${imageId}`, {
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

export default ImageService;
