import jwtdecode from "jwt-decode";
import { Authentication } from "../Routes";
import { fLocalStorage as localStorage } from "../utilities/LocalStorageWrapper";
import { apiFetch, apiFetchTyped, getGeneralError } from "./Helpers";
import { IShortAgreementDetails } from "./interfaces/AgreementInterfaces";
import { IApiResponse, IBasicResponse, IErrorResponse } from "./interfaces/Common";
import { IShortFlatDetails } from "./interfaces/FlatServiceInterfaces";
import {
  ILoginResponse,
  IRegisterRequest,
  ITokenPayload,
  IUserAgreements,
  IUserDetails,
} from "./interfaces/UserInterfaces";

export enum Roles {
  Administrator = 1,
  User = 2,
}

export const Policies = {
  Administrator: [Roles.Administrator],
  User: [Roles.Administrator, Roles.User],
};

class UserService {
  public static async getUserData(userId: string): Promise<IApiResponse<IUserDetails>> {
    try {
      const [result, parsed] = await apiFetchTyped<IUserDetails>(
        `/api/user/${userId}`,
        {
          method: "GET",
        },
        true
      );

      return parsed;
    } catch (e) {
      console.log(e);
      return getGeneralError<IUserDetails>();
    }
  }

  public static async getUserAgreementsTenant(userId: string): Promise<IApiResponse<IShortAgreementDetails[]>> {
    try {
      const [result, parsed] = await apiFetchTyped<IShortAgreementDetails[]>(
        `/api/user/${userId}/agreements/tenant`,
        {
          method: "GET",
        },
        true
      );

      return parsed;
    } catch (e) {
      console.log(e);
      return getGeneralError<IShortAgreementDetails[]>();
    }
  }

  public static async getUserAgreementsOwner(userId: string): Promise<IApiResponse<IShortAgreementDetails[]>> {
    try {
      const [result, parsed] = await apiFetchTyped<IShortAgreementDetails[]>(
        `/api/user/${userId}/agreements/owner`,
        {
          method: "GET",
        },
        true
      );

      return parsed;
    } catch (e) {
      console.log(e);
      return getGeneralError<IShortAgreementDetails[]>();
    }
  }

  public static async getUserFlats(userId: string, offset: number): Promise<IApiResponse<IShortFlatDetails[]>> {
    try {
      const [result, parsed] = await apiFetchTyped<IShortFlatDetails[]>(
        `/api/user/${userId}/flats?offset=${offset}`,
        {
          method: "GET",
        },
        true
      );

      return parsed;
    } catch (e) {
      console.log(e);
      return getGeneralError<IShortFlatDetails[]>();
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

  public static async register(model: IRegisterRequest): Promise<{ [key: string]: string[] }> {
    try {
      const result = await apiFetch("/api/user/register", {
        body: JSON.stringify(model),
        method: "POST",
      });
      if (result.ok) {
        return {};
      } else {
        const response = (await result.json()) as IBasicResponse;
        if (response.errors !== undefined) {
          return response.errors;
        }
      }
    } catch (e) {
      console.log(e);
    }
    return { General: ["Įvyko nežinoma klaida"] };
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
  };
  public static readonly token = (): string | null => localStorage.getItem("Token");
  public static readonly canEdit = (ownerId: string): boolean => ownerId === UserService.userId();
  public static readonly satisfiesAuthentication = (authLevel: Authentication) =>
    (UserService.isLoggedIn() && authLevel === Authentication.Authenticated) ||
    (!UserService.isLoggedIn() && authLevel === Authentication.Anonymous) ||
    authLevel === Authentication.Either;

  public static readonly authorizationHeaders = (): { [key: string]: string } => ({
    Authorization: `Bearer ${UserService.token()}`,
  });

  private static setToken(token: string): void {
    const decoded = jwtdecode(token) as ITokenPayload;

    if (!Array.isArray(decoded.role)) {
      decoded.role = [decoded.role as string];
    }

    localStorage.setItems(
      { key: "Token", value: token },
      { key: "Roles", value: JSON.stringify(decoded.role) },
      { key: "UserId", value: decoded.UserId },
      { key: "TokenExpires", value: decoded.exp.toString() }
    );
  }

  private static clearToken(): void {
    localStorage.removeItems("Token", "Roles", "UserId", "TokenExpires");
  }
}

export default UserService;
