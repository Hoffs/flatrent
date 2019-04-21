import { apiFetch } from "./Helpers";
import { IErrorResponse, IBasicResponse, IFlatAddress, IUserDetails } from "./Settings";
import UserService from "./UserService";
import ImageService from "./ImageService";

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
  totalFloors: string;
  roomCount: string;
  price: string;
  yearOfConstruction: string;
  minimumRentDays: number;
  isFurnished: boolean;
  description: string;
  tenantRequirements: string;

  street: string;
  houseNumber: string;
  flatNumber: string;
  city: string;
  country: string;
  postCode: string;

  features: string[];
  images: Array<{ name: string }>;
}

export interface IFlatCreateResponse {
  id: string;
  images: { [key: string]: string };
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

  public static async createFlat(requestData: { [key: string]: string | boolean }, images: File[]): Promise<IBasicResponse> {
    const data: IBasicResponse = {};
    const request = requestData as unknown as IFlatCreateData;
    const featuresString = requestData.features as string;
    if (featuresString.split !== undefined) {
      request.features = (requestData.features as string).split(",");
    }
    request.images = images.map((i) => ({ name: i.name }));
    try {
      const result = await apiFetch(`/api/flat/`, {
        body: JSON.stringify(requestData),
        headers: UserService.authorizationHeaders(),
        method: "POST",
      });
      if (result.ok) {
        const response = (await result.json()) as IFlatCreateResponse;
        const promises = Object.keys(response.images).map((key) => {
          const image = images.find((img) => img.name === key);
          return ImageService.putFlatImage(response.images[key], image!);
        });
        await Promise.all(promises);
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
    console.log("end")

    data.errors = { General: ["Įvyko nežinoma klaida"] };
    return data;
  }
}

export default FlatService;
