import { toast } from "react-toastify";
import { history } from "../MainRouter";
import { loginUrl } from "../utilities/Utilities";
import { IApiResponse, IBasicResponse, IErrorResponse } from "./interfaces/Common";
import { ApiHostname, DefaultHeaders } from "./Settings";
import UserService from "./UserService";

export const makeUrl = (base: string, path: string): string => {
  if (base.endsWith("/") && path.startsWith("/")) {
    return base.concat(path.substring(1));
  }
  return base.concat(path);
};

export const apiFetch = async (
  path: string,
  init?: RequestInit | undefined,
  withAuth: boolean = false
): Promise<Response> => {
  if (init !== undefined) {
    init.headers = {
      ...init.headers,
      ...DefaultHeaders,
    };
  } else {
    init = {
      headers: {
        ...DefaultHeaders,
      },
    };
  }

  if (withAuth) {
    init.headers = {
      ...init.headers,
      ...UserService.authorizationHeaders(),
    };
  }

  const response = await fetch(makeUrl(ApiHostname, path), init);

  if (withAuth) {
    if (response.status === 401) {
      UserService.logout();
      toast.error("Jūsų sesija baigėsi.");
      history.push(loginUrl());
    }
  }

  return response;
};

export async function apiFetchTyped<T>(
  path: string,
  init?: RequestInit | undefined,
  withAuth: boolean = false
): Promise<[Response, IApiResponse<T>]> {
  if (init !== undefined) {
    init.headers = {
      ...init.headers,
      ...DefaultHeaders,
    };
  } else {
    init = {
      headers: {
        ...DefaultHeaders,
      },
    };
  }

  if (withAuth) {
    init.headers = {
      ...init.headers,
      ...UserService.authorizationHeaders(),
    };
  }

  const response = await fetch(makeUrl(ApiHostname, path), init);

  if (withAuth) {
    if (response.status === 401) {
      UserService.logout();
      toast.error("Jūsų sesija baigėsi.");
      history.push(loginUrl());
    }
  }

  const json = await response.json();
  const apiResponse: IApiResponse<T> = {};
  if (response.ok) {
    apiResponse.data = json as T;
  } else {
    const bresponse = json as IBasicResponse;
    console.log(bresponse.errors);
    if (bresponse.errors === undefined) {
      bresponse.errors = { general: ["Įvyko nežinoma klaida"] };
    } else {
      apiResponse.errors = bresponse.errors;
    }
  }

  return [response, apiResponse];
}

export const uploadEach = async (
  toUpload: { [key: string]: string },
  files: File[],
  func: (id: string, file: File) => Promise<IBasicResponse>
): Promise<IErrorResponse | undefined> => {
  console.log(toUpload);
  console.log(files);
  const promises = Object.keys(toUpload).map((apiFileId) => {
    const file = files.find((f) => f.name === toUpload[apiFileId]);
    console.log(file, apiFileId);
    return func(apiFileId, file!);
  });

  const results = await Promise.all(promises);
  let errorsOb: IErrorResponse = {};
  results
    .map((r) => r.errors)
    .forEach((err) => {
      if (err === undefined) {
        return;
      }
      errorsOb = { ...errorsOb, ...err };
    });

  if (Object.keys(errorsOb).length === 0) {
    return undefined;
  }
  return errorsOb;
};

export function getGeneralError<T>(): IApiResponse<T> {
  return { errors: { general: ["Įvyko nežinoma klaida"] } };
}
