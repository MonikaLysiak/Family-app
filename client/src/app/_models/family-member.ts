import { Photo } from "./photo";

export interface FamilyMember {
    id: number;
    userName: string;
    name: string;
    surname: string;
    photoUrl: string;
    age: number;
    nickname: string;
    created: Date;
    lastActive: Date;
    gender: string;
    photos: Photo[];
  }