import { IErrorResponse, IAddress, IUserDetails, IFileMetadata, IBasicResponse } from "./Common";

// FlatList interfaces
export interface IFlatListItem {
  id: string;
  imageId: string;
  name: string;
  area: number;
  floor: number;
  roomCount: number;
  price: number;
  address: IShortAddress;
}

export interface IShortAddress {
  street: string;
  city: string;
  country: string;
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
  address: IAddress;
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

// Create Flat interfaces

export interface IFlatCreateRequest {
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
  images: IFileMetadata[];
}

export interface IFlatCreateResponse {
  id: string;
  images: { [key: string]: string };
}

export interface IRentRequest {
  from: string;
  to: string;
  comments: string;
  attachments: IFileMetadata[];
}

export interface IAgreementCreateResponse {
  id: string;
  attachments: { [key: string]: string };
}
