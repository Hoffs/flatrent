import { apiFetch } from "./Helpers";
import { IErrorResponse, IBasicResponse, IFlatAddress, IUserDetails } from "./Settings";
import UserService from "./UserService";

// FlatList interfaces
export interface IFlatListItem {
  id: string;
  name: string;
  area: number;
  floor: number;
  roomCount: number;
  price: number;
  yearOfConstruction: number;
  address: IFlatAddress;
  owner: IUserDetails;
}

export interface IFlatListResponse {
  flats?: IFlatListItem[];
  errors?: IErrorResponse;
}

// Flat Detail intertfaces
export interface IFlatDetails {
  id: string;
  name: string;
  area: number;
  floor: number;
  roomCount: number;
  price: number;
  yearOfConstruction: number;
  isFurnished: boolean;
  features: string[];
  description: string;
  address: IFlatAddress;
  owner: IUserDetails;
  tenantRequirements: string;
  minimumRentDays: number;
  isPublished: boolean;
  isPublic: boolean;
  isRented: boolean;
  isAvailableForRent: boolean;
  images: IImageDetails[];
}

export interface IImageDetails {
  name: string;
  id: string;
}

export interface IFlatDetailsResponse {
  flat?: IFlatDetails;
  errors?: IErrorResponse;
}

// Create Flat interfaces

export interface IFlatCreateData {
  name: string;
  area: string;
  floor: string;
  roomCount: string;
  price: string;
  yearOfConstruction: string;
  description: string;

  ownerName: string;
  account: string;
  email: string;
  phoneNumber: string;

  street: string;
  houseNumber: string;
  flatNumber: string;
  city: string;
  country: string;
  postCode: string;
}

export const getAddressString = (address: IFlatAddress) => {
  console.log(address);
  return `${address.street} ${address.houseNumber}-${address.flatNumber}, ${address.city}, ${address.country}`;
};

class FlatService {
  public static async getFlats(count: number, offset: number, rented: boolean = false): Promise<IFlatListResponse> {
    const data: IFlatListResponse = {};
    try {
      const rentedQuery = rented ? "&rented=true" : "";
      const result = await apiFetch(`/api/flat?count=${count}&offset=${offset}${rentedQuery}`, {
        headers: UserService.authorizationHeaders(),
      });
      if (result.ok) {
        console.log("got flats");
        const response = (await result.json()) as IFlatListItem[];
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

  public static async getFlat(id: string): Promise<IFlatDetailsResponse> {
    const data: IFlatDetailsResponse = {};
    try {
      const result = await apiFetch(`/api/flat/${id}`, {
        headers: UserService.authorizationHeaders(),
      });
      if (result.ok) {
        console.log("got flat");
        const response = (await result.json()) as IFlatDetails;
        data.flat = response;
      } else {
        console.log("didnt get flat");
        const response = (await result.json()) as IErrorResponse;
        data.errors = response;
      }
    } catch (e) {
      console.log(e);
      data.errors = { General: ["Įvyko nežinoma klaida"] };
    }

    return data;
  }

  public static async rentFlat(id: string, from: string, to: string): Promise<IBasicResponse> {
    const data: IBasicResponse = {};
    try {
      const result = await apiFetch(`/api/flat/${id}/rent`, {
        body: JSON.stringify({ from, to }),
        headers: UserService.authorizationHeaders(),
        method: "POST",
      });

      if (result.ok) {
        console.log("got flat");
      } else {
        console.log("didnt get flat");
        const response = (await result.json()) as IErrorResponse;
        data.errors = response;
      }
    } catch (e) {
      console.log(e);
      data.errors = { General: ["Įvyko nežinoma klaida"] };
    }

    return data;
  }

  public static async createFlat(requestData: IFlatCreateData): Promise<IBasicResponse> {
    const data: IBasicResponse = {};
    try {
      const result = await apiFetch(`/api/flat/`, {
        body: JSON.stringify(requestData),
        headers: UserService.authorizationHeaders(),
        method: "POST",
      });
      if (result.ok) {
        console.log("created flat");
      } else {
        console.log("didnt create flat");
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
