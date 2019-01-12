import jwtdecode from "jwt-decode";
import { apiFetch } from "./Helpers";
import { IErrorResponse } from "./Settings";

export enum Roles {
  Administrator = "Administrator",
  Client = "Client",
  Employee = "Employee",
  Supply = "Supply",
  Accounting = "Accounting",
  CustomerService = "CustomerService",
  Sales = "Sales",
}

export const Policies = {
  Accounting: ["Administrator", "Accounting"],
  Administrator: ["Administrator"],
  Client: ["Administrator", "Client"],
  CustomerService: ["Administrator", "CustomerService"],
  Employee: ["Administrator", "Employee"],
  Sales: ["Administrator", "Sales"],
  Supply: ["Administrator", "Supply"],
};

interface ILoginResponse {
  token?: string;
}

interface ITokenPayload {
  UserId: string;
  UserType: string;
  role: string | string[];
  exp: number;
}

class UserService {
  public static async authenticate(email: string, password: string): Promise<{ [key: string]: string[] }> {
    try {
      const result = await apiFetch("/api/user/login", {
        body: JSON.stringify({ email, password }),
        method: "POST",
      });
      if (result.ok) {
        const response = (await result.json()) as ILoginResponse;
        if (response.token !== undefined) {
          this.setToken(response.token);
          return {};
        } else {
          console.log(response);
          return { General: ["Įvyko nežinoma klaida"] };
        }
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

  public static getRoles(): string[] {
    const item = localStorage.getItem("Roles");
    return item ? (JSON.parse(item) as string[]) : [];
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

  public static satisfiesRoles(...roles: string[]): boolean {
    if (roles.length === 0) {
      return true;
    }
    const userRoles = this.getRoles();
    if (userRoles.length === 0) {
      return false;
    }

    return userRoles.some((role) => roles.includes(role));
  }

  public static readonly token = (): string | null => localStorage.getItem("Token");

  public static readonly authorizationHeaders = (): { [key: string]: string } => ({
    Authorization: `Bearer ${UserService.token()}`,
  })

  private static setToken(token: string): void {
    const decoded = jwtdecode(token) as ITokenPayload;
    if (!Array.isArray(decoded.role)) {
      decoded.role = [decoded.role as string];
    }
    localStorage.setItem("Token", token);
    localStorage.setItem("Roles", JSON.stringify(decoded.role));
    localStorage.setItem("UserId", decoded.UserId);
    localStorage.setItem("UserType", decoded.UserType);
    localStorage.setItem("TokenExpires", decoded.exp.toString());
  }

  private static clearToken(): void {
    localStorage.removeItem("Token");
    localStorage.removeItem("Roles");
    localStorage.removeItem("UserId");
    localStorage.removeItem("UserType");
    localStorage.removeItem("TokenExpires");
  }
}

export default UserService;
