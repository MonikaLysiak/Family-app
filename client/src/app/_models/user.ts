import { AuthStatus } from "../_enums/auth-status";

export interface User {
    username: string;
    token: string;
    photoUrl: string;
    name: string;
    roles: string[];
}

export interface AuthResponse {
    status: AuthStatus;
    user?: User;
  }