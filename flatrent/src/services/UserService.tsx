import jwtdecode from "jwt-decode";
import { Authentication } from "../Routes";
import { fLocalStorage as localStorage } from "../utilities/LocalStorageWrapper";
import { apiFetch, apiFetchTyped } from "./Helpers";
import { IAddress, IBasicResponse, IErrorResponse, IApiResponse } from "./interfaces/Common";

export enum Roles {
  Administrator = 1,
  User = 2,
}

export const Policies = {
  Administrator: [Roles.Administrator],
  User: [Roles.Administrator, Roles.User],
};

interface ILoginResponse {
  token: string;
}

interface ITokenPayload {
  UserId: string;
  role: string | string[];
  exp: number;
}

export interface IRegistrationModel {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phoneNumber: string;
}

export interface IUserData {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  clientInformation?: IClientData;
}

export interface IClientData {
  id: string;
  description: string;
}

export interface IAgreementData {
  id: string;
  from: string;
  to: string;
  flatName: string;
  flatAddress: IAddress;
}

export interface IUserAgreements {
  owner: IAgreementData[];
  tenant: IAgreementData[];
}

class UserService {
  public static async getUserData(): Promise<IUserData | IErrorResponse> {
    try {
      const result = await apiFetch("/api/user", {
        headers: this.authorizationHeaders(),
        method: "GET",
      });

      if (result.ok) {
        const response = (await result.json()) as IUserData;
        return response;
      } else {
        const response = (await result.json()) as IErrorResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return { General: ["Įvyko nežinoma klaida"] };
    }
  }

  public static async getUserAgreements(): Promise<IUserAgreements | IErrorResponse> {
    try {
      const result = await apiFetch("/api/user/agreements", {
        headers: this.authorizationHeaders(),
        method: "GET",
      });

      if (result.ok) {
        if (result.status === 204) {
          return { owner: [], tenant: [] };
        }

        const response = (await result.json()) as IUserAgreements;
        return response;
      } else {
        const response = (await result.json()) as IErrorResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return { General: ["Įvyko nežinoma klaida"] };
    }
  }

  public static async authenticate(email: string, password: string): Promise<IApiResponse<ILoginResponse>> {
    try {
      const [result, parsed] = await apiFetchTyped<ILoginResponse>("/api/user/login", {
        body: JSON.stringify({ email, password }),
        method: "POST",
      });
      console.log(parsed);
      if (parsed.data !== undefined) {
        this.setToken(parsed.data.token);
      }
      return parsed;

    } catch (e) {
      console.log(e);
      return { errors: { General: ["Įvyko nežinoma klaida"] } };
    }
  }

  public static async register(model: IRegistrationModel): Promise<{ [key: string]: string[] }> {
    try {
      const result = await apiFetch("/api/user/register", {
        body: JSON.stringify(model),
        method: "POST",
      });
      if (result.ok) {
        return {};
      } else {
        const response = (await result.json()) as IErrorResponse;
        return response;
      }
    } catch (e) {
      console.log(e);
      return { General: ["Įvyko nežinoma klaida"] };
    }
  }

  public static async refresh(): Promise<{ [key: string]: string[] }> {
    let fResponse: { [key: string]: string[] } = {};
    try {
      const result = await apiFetch("/api/user/refresh", {
        method: "POST",
      });
      if (result.ok) {
        const response = (await result.json()) as ILoginResponse;
        if (response.token !== undefined) {
          this.setToken(response.token);
          fResponse = {};
        } else {
          console.log(response);
          fResponse = { General: ["Įvyko nežinoma klaida"] };
        }
      } else {
        const response = (await result.json()) as IErrorResponse;
        fResponse = response;
      }
    } catch (e) {
      console.log(e);
      fResponse = { General: ["Įvyko nežinoma klaida"] };
    }

    if (fResponse !== {}) {
      this.clearToken();
    }
    return fResponse;
  }

  public static getRoles(): number[] {
    const item = localStorage.getItem("Roles");
    return item ? (JSON.parse(item) as string[]).map((i) => Number.parseInt(i, 10)) : [];
  }

  public static isLoggedIn(): boolean {
    const token = localStorage.getItem("Token");
    const exp = Number(localStorage.getItem("TokenExpires"));
    const now = Date.now() / 1000;
    return exp > now && token !== null;
  }

  public static logout(): void {
    this.clearToken();
  }

  public static hasRoles(...roles: number[]): boolean {
    if (roles.length === 0) {
      return true;
    }
    const userRoles = this.getRoles();
    if (userRoles.length === 0) {
      return false;
    }

    return userRoles.some((role) => roles.includes(role));
  }

  public static readonly userId = (): string => {
    const userId = localStorage.getItem("UserId");
    return userId ? userId : "";
  }
  public static readonly token = (): string | null => localStorage.getItem("Token");
  public static readonly canEdit = (ownerId: string): boolean => ownerId === UserService.userId();
  public static readonly satisfiesAuthentication = (authLevel: Authentication) =>
    (UserService.isLoggedIn() && authLevel === Authentication.Authenticated)
    || (!UserService.isLoggedIn() && authLevel === Authentication.Anonymous)
    || authLevel === Authentication.Either

  public static readonly authorizationHeaders = (): { [key: string]: string } => ({
    Authorization: `Bearer ${UserService.token()}`,
  })

  private static setToken(token: string): void {
    const decoded = jwtdecode(token) as ITokenPayload;

    if (!Array.isArray(decoded.role)) {
      decoded.role = [decoded.role as string];
    }

    localStorage.setItems(
      {key: "Token", value: token},
      {key: "Roles", value: JSON.stringify(decoded.role)},
      {key: "UserId", value: decoded.UserId},
      {key: "TokenExpires", value: decoded.exp.toString()},
    );
  }

  private static clearToken(): void {
    localStorage.removeItems("Token", "Roles", "UserId", "TokenExpires");
  }
}

export default UserService;
