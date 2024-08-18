import { ListItem } from "./list-item";

export interface FamilyList {
    id: number | null;
    familyId: number;
    name: string;
    categoryId: number;
    listItems: ListItem[];
}