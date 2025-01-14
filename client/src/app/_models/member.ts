import { Photo } from "./photo";

export interface Member {
    id: number;
    userName: string;
    photoUrl: string;
    age: number;
    nickname: string;
    created: Date;
    lastActive: Date;
    gender: string;
    name: string;
    surname: string;
    photos: Photo[];
  }