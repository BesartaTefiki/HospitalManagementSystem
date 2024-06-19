<<<<<<< HEAD
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

=======
# Getting Started with Create React App

This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

## Available Scripts

In the project directory, you can run:

### `npm start`

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### `npm test`

Launches the test runner in the interactive watch mode.\
See the section about [running tests](https://facebook.github.io/create-react-app/docs/running-tests) for more information.

### `npm run build`

Builds the app for production to the `build` folder.\
It correctly bundles React in production mode and optimizes the build for the best performance.

The build is minified and the filenames include the hashes.\
Your app is ready to be deployed!

See the section about [deployment](https://facebook.github.io/create-react-app/docs/deployment) for more information.

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

### Code Splitting

This section has moved here: [https://facebook.github.io/create-react-app/docs/code-splitting](https://facebook.github.io/create-react-app/docs/code-splitting)

### Analyzing the Bundle Size

This section has moved here: [https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size](https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size)

### Making a Progressive Web App

This section has moved here: [https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app](https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app)

### Advanced Configuration

This section has moved here: [https://facebook.github.io/create-react-app/docs/advanced-configuration](https://facebook.github.io/create-react-app/docs/advanced-configuration)

### Deployment

This section has moved here: [https://facebook.github.io/create-react-app/docs/deployment](https://facebook.github.io/create-react-app/docs/deployment)

### `npm run build` fails to minify

This section has moved here: [https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify](https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify)
>>>>>>> 5d7d9f7 (Initialize project using Create React App)
