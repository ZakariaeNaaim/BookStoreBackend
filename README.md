# BookStore Web API

This is an **E-Commerce Book Store Web API** built with **ASP.NET Core Web API (.NET 10)**, **C#**, and **Entity Framework Core (Code First)**.  
It exposes RESTful endpoints for product management, shopping carts, orders, and secure online payments with **Stripe Checkout**.  
The project follows **Clean Architecture**  for scalability and maintainability.

---

## 🚀 Features

### Product Management
- Users can browse books and add them to their cart via API endpoints.
- Admins can manage product listings (CRUD operations).
- Real-time updates to products are handled through EF Core migrations and SQL views.

### User Roles & Authentication
- Four types of accounts: **Admin, Employee, Customer, Company**.
- Secure authentication with Facebook and Microsoft login integrations.

### Admin API
- Admin and Employee roles can manage products, orders, and user accounts.
- Endpoints for order management and user administration.

### Online Payments
- Integrated **Stripe Checkout** for secure online payments.
- Orders store `SessionId` and `PaymentIntentId` for later verification.

---

## 🧩 Technologies Used
- **Backend**: ASP.NET Core Web API (.NET 10)
- **Database**: SQL Server, Entity Framework Core (Code First)
- **Payment Integration**: Stripe
- **Authentication**: JWT Bearer Tokens
- **Design Patterns**: Repository, Unit of Work
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, WebApi)

---

## ⚙️ Prerequisites
- **.NET SDK**: Version 10.0 or later
- **SQL Server**: Local or remote instance
- **Visual Studio 2026**

---

## 🚀 Getting Started

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/bookstore-webapi.git
2. Open the Project
   Open the project in your preferred IDE. Visual Studio 2026 is recommended for full ASP.NET Core and EF Core support. Simply open the solution file (.sln) in Visual Studio.

3. Configure the Database
   Open the appsettings.json file located in the root of the project.
   Update the connection string under "ConnectionStrings": { "DefaultConnection": "Your_SQL_Server_Connection_String" } to match your SQL Server configuration.

4. Build the Application
   Navigate to the project directory and build the application: dotnet build
   
6. Run the Application
    Simply run the application to initialize the database and start the server. The application is configured to check for any pending migrations on startup and apply them automatically if needed.

    dotnet run
