# Family App

Family App is a web application designed to simplify daily family management and communication. Built with the MVVM architecture and RESTful services, it integrates front-end and back-end technologies, including Angular, ASP.NET, and SQLite.

## Features

- **Family Groups** – Create family groups and invite members.  
- **Real-Time Communication** – Family chat powered by SignalR for seamless interaction.  
- **Photo Gallery** – Upload, view, and manage family photos with Cloudinary integration.  
- **Task & List Management** – Shared lists for groceries, chores, and more.  
- **User Profiles** – Edit personal details and change language settings (English/Polish).  
- **Role Management** – Admins can manage user roles and permissions.  

## Technologies Used

- **Front-end:** Angular, Bootstrap, ngx-bootstrap, CSS  
- **Back-end:** ASP.NET Core, SQLite, ASP.NET Core Identity  
- **Real-time Updates:** SignalR  
- **Cloud Storage:** Cloudinary  
- **Routing & Forms:** @angular/router, Reactive Forms  

## Implementation Overview

### Home Page
The landing page allows users to log in, create an account, and switch languages.  
![home](https://github.com/user-attachments/assets/ea90a673-eb79-48fe-b418-82a45c7a9baf)

### Family Dashboard
Displays a list of families, provides navigation, and enables new family creation.  
![home_logged_in](https://github.com/user-attachments/assets/7dedde35-d1b6-4b08-a54a-45a6082cde67)

### User Identity & Admin Panel
User roles are managed using ASP.NET Core Identity, where admins can assign permissions.  
![admin_with_edit](https://github.com/user-attachments/assets/c6cb6c63-befd-436c-a537-162cc6399d80)

### Family Members & Profiles
Each member has a profile with a status indicator (online/offline).  
![family_members](https://github.com/user-attachments/assets/c9e37387-bb50-4a2e-a4af-9ba719b6c31a)

### Invitations
Send and manage invitations to join families.  
![invitations_received_1](https://github.com/user-attachments/assets/b4327cb8-ce76-4958-a223-66ad716a0a59)

### Task Lists
Create, edit, and track shared family task lists.
![family_lists](https://github.com/user-attachments/assets/6896d7ea-bf00-442d-8605-34e64fe2ca31)

### Photo Gallery
Upload and manage images using Cloudinary integration.  
![family_photos_gallery](https://github.com/user-attachments/assets/e5a6f43a-6e90-425c-8ab8-424ba855b90c)

### Family Chat
Real-time messaging for family members, powered by SignalR.
![family_chat_new](https://github.com/user-attachments/assets/ee708d14-a143-4b7f-bddf-c0c457f16e41)

## Installation & Setup

1. Clone the repository:  
git clone https://github.com/MonikaLysiak/Family-app.git

2. Navigate to the project folder and install dependencies:  
cd client
npm install

3. Run the Angular front-end:  
ng serve

4. Start the ASP.NET back-end:  
cd API
dotnet run
