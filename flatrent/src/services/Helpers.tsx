import { toast } from "react-toastify";
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
    }
  }

  return response;
};
