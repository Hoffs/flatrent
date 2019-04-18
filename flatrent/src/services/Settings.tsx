export const ApiHostname = "https://localhost:5001/";
export const DefaultHeaders = {
  "Content-Type": "application/json",
};

export interface IErrorResponse {
  [key: string]: string[];
}

// Basic Response

export interface IBasicResponse {
  errors?: IErrorResponse;
}

// General Flat

export interface IFlatAddress {
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
