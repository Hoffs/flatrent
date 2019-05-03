import { SpawnSyncOptionsWithStringEncoding } from "child_process";

export interface IErrorResponse {
  [key: string]: string[];
}

// Basic Response

export interface IBasicResponse {
  errors?: IErrorResponse;
  message?: string;
}

export interface IFileMetadata {
  name: string;
}

export interface IAttachment {
  id: string;
  name: string;
  mime: string;
}

export interface IFileResponse {
  [key: string]: string;
}

export interface IApiResponse<T> extends IBasicResponse {
  data?: T;
}
