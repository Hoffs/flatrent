import { apiFetch } from "./Helpers";
import { IErrorResponse } from "./Settings";
import UserService from "./UserService";

export interface IFlatInfo {
  id: string;
  name: string;
  area: number;
  floor: number;
  roomCount: number;
  price: number;
  yearOfConstruction: number;
  address: IFlatAddress;
}

export interface IFlatAddress {
  street: string;
  houseNumber: string;
  flatNumber: string;
  city: string;
  country: string;
  postCode: string;
}

export interface IFlatListResponse {
  flats?: IFlatInfo[];
  errors?: IErrorResponse;
}

export const getAddressString = (address: IFlatAddress) => {
  return `${address.street} ${address.houseNumber}-${address.flatNumber}, ${address.city}, ${address.country}`;
};

class FlatService {
  public static async getFlats(count: number, offset: number, rented: boolean = false): Promise<IFlatListResponse> {
    const data: IFlatListResponse = {};
    try {
      const result = await apiFetch(`/api/flat?count=${count}&offset=${offset}`, {
        headers: UserService.authorizationHeaders(),
      });
      if (result.ok) {
        console.log("got flats");
        const response = (await result.json()) as IFlatInfo[];
        data.flats = response;
      } else {
        console.log("didnt get flats");
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

export default FlatService;
