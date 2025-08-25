# 📚 Library Management System - REST API + Frontend

A complete **library management system**, with a **.NET REST API** and a **modern HTML/CSS/JS frontend**.

---

## 🛠️ Technologies Used

### 🔧 Backend
- .NET 8 / 9 – Main framework  
- ASP.NET Core – Web API framework  
- Swagger/OpenAPI – API documentation  
- C# 11 – Programming language  

### 🎨 Frontend
- HTML5 – Page structure  
- CSS3 – Styling and responsive design  
- JavaScript (ES6+) – Interactivity and API consumption  
- Font Awesome – Icons  
- Flexbox / Grid – Modern layout  

---

## ✨ Features

### 🔧 Backend (.NET API)
- ✅ Complete CRUD for **books, users, and loans**  
- ✅ RESTful API with HTTP endpoints  
- ✅ Automatic documentation with Swagger  
- ✅ Data validation and error handling  

### 🎨 Frontend (HTML/CSS/JS)
- ✅ Modern and responsive interface  
- ✅ Book management with filters and search  
- ✅ User control  
- ✅ Complete loan system  
- ✅ Responsive design for mobile devices  
- ✅ Modals for adding/editing resources  

---

### 🚀 How to Run

### Prerequisites
- SDK **.NET 8 or 9** installed  

### Steps

# 1) Navigate to API folder
cd BibliotecaAPI

# 2) Restore dependencies
dotnet restore

# 3) Run the API
dotnet run

# 4) Open index.html in the Frontend directory
🎯 Frontend Features
📖 Book Management
Listing with informative cards

Availability filters

Search by title, author, or genre

Modal to add new books

👥 User Management
List of registered users

Search by name or email

Modal to add new users

🔄 Loan Control
View active loans

Return management

Modal to create new loans

Status filters

🔗 API Endpoints
The API provides full CRUD functionality. Examples below:

📚 Books
http
Copiar
Editar
# Get all books
GET /api/books

# Get book by ID
GET /api/books/{id}

# Add a new book
POST /api/books
Content-Type: application/json
{
  "title": "The Pragmatic Programmer",
  "author": "Andrew Hunt",
  "genre": "Programming",
  "available": true
}

# Update book
PUT /api/books/{id}

# Delete book
DELETE /api/books/{id}
👥 Users
http
Copiar
Editar
# Get all users
GET /api/users

# Register new user
POST /api/users
Content-Type: application/json
{
  "name": "John Doe",
  "email": "john.doe@email.com"
}
🔄 Loans
http
Copiar
Editar
# Get active loans
GET /api/loans

# Create a new loan
POST /api/loans
Content-Type: application/json
{
  "userId": 1,
  "bookId": 2,
  "loanDate": "2025-08-21",
  "returnDate": null
}

# Return a book
PUT /api/loans/{id}/return
🐚 Example with cURL
bash
Copiar
Editar
# Get all books
curl -X GET https://localhost:5001/api/books

# Add a new book
curl -X POST https://localhost:5001/api/books \
  -H "Content-Type: application/json" \
  -d '{"title":"Clean Code","author":"Robert C. Martin","genre":"Programming","available":true}'
⭐ Contribution
If this project helped you, don’t forget to leave a ⭐ on GitHub!

📌 Follow me on LinkedIn: Enzo Spíndola
