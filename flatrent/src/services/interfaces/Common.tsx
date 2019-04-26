
export interface IErrorResponse {
  [key: string]: string[];
}

// Basic Response

export interface IBasicResponse {
  errors?: IErrorResponse;
  message?: string;
}

// General Flat

export interface IAddress {
  street: string;
  houseNumber: string;
  flatNumber: string;
  city: string;
  country: string;
  postCode: string;
}

export interface IUserDetails {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
}

export interface IFileMetadata {
  name: string;
}

export interface IApiResponse<T> extends IBasicResponse {
  data?: T;
}
