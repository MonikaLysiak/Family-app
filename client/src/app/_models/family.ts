import { Photo } from "./photo";

export interface Family {
    id: number;
    photoUrl: string;
    name: string;
    familyPhotos: Photo[];
    userNickname: string;
}