export interface Message {
    id: number;

    senderId: number;
    senderUsername: string;
    senderPhotoUrl: string;

    familyId: number;

    content: string;
    messageSent: Date;
  }