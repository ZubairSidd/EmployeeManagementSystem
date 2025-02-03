# Employee Management System

A web-based employee management system built with ASP.NET Core 8 Razor Pages and PostgreSQL, implementing SOLID principles and repository pattern.

## Features

### User Management
- User registration and login
- Admin/Employee role separation
- Secure password hashing
- Session-based authentication

### Employee Features
- Personal profile management
- Profile picture upload (Base64 storage)
- Basic information management (Name, DOB, Employee ID, etc.)
- Password change functionality

### Admin Features
- View all employees
- Create/Update/Delete employee records
- Admin dashboard
- Employee profile management

### Security
- Password hashing
- Session management
- Role-based access control
- Admin registration with secure code

## Technology Stack

- ASP.NET Core 8.0
- PostgreSQL
- Entity Framework Core
- Bootstrap 5
- Razor Pages

## Project Structure
├── Data/
│   ├── AppDbContext.cs        # Database context
│   └── Repositories/          # Repository implementations
├── Models/
│   └── Interfaces/           # All interfaces
└── Pages/                   # Razor pages
│   └── Account/  
│   └── Admin/         

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 or VS Code

### Database Configuration

1. Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=employeemanagement;Username=your_username;Password=your_password"
  }
}

#Apply database migrations:
dotnet ef database update

##Running the Application

#1. Clone the repository

git clone [repository-url]

#2. Navigate to the project directory
cd EmployeeManagementSystem

#3. Run the application
dotnet run

##Admin Registration
#To register as an admin:

#1. Go to the signup page
#2. Check "Register as Admin"
#3. Use the admin registration code from appsettings.json
