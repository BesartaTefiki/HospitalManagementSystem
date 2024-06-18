# Hospital Management System

## Overview

The **Hospital Management System (HMS)** is designed to streamline and automate the processes involved in hospital management. It helps manage patient records, doctor appointments, and prescriptions efficiently and securely. The project uses a service-oriented architecture with a backend developed in C# using ASP.NET Core and a frontend built with React.

## Features

- **User Management**: Register and login functionalities for patients and doctors.
- **Appointment Management**: Patients can book, view, and manage appointments.
- **Prescription Management**: Doctors can create, update, and delete prescriptions.
- **Role-Based Access**: Secure authentication and authorization using roles (Patient, Doctor).

## Technologies Used

### Backend
- **Language**: C#
- **Framework**: ASP.NET Core (version 7.0)
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **IDE**: Visual Studio

### Frontend
- **Library**: React
- **IDE**: Visual Studio Code

### Development Tools
- **Version Control**: Git (hosted on GitHub)
- **Database Management**: pgAdmin 4

## Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js](https://nodejs.org/en/)
- [PostgreSQL](https://www.postgresql.org/download/)
- [pgAdmin 4](https://www.pgadmin.org/download/)

### Setup

1. **Clone the Repository**

```sh
git clone https://github.com/your-username/hospital-management-system.git
cd hospital-management-system

Navigate to the backend project directory
Install the required packages:
Update the appsettings.json file with your PostgreSQL connection string:

"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=HospitalDB;Username=your-username;Password=your-password"
}

Apply migrations to set up the database schema
Run the backend server
Install the required packages:

npm install
Run the frontend development server:
npm start
Open your browser and navigate to http://localhost:3000 to access the frontend.
The backend API will be running on https://localhost:7161.

