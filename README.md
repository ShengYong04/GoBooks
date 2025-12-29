# 📚 GoBooks - ASP.NET MVC E-Commerce Store

**GoBooks** is a robust, full-stack e-commerce application designed for selling books online. Built with **C# ASP.NET MVC 5** and **Entity Framework**, it demonstrates a modern, responsive user experience using **Bootstrap 5** and **jQuery AJAX** for seamless interactions without page reloads.

![Project Banner](https://via.placeholder.com/1000x400?text=GoBooks+Homepage+Screenshot)
*(Note: Upload your 'image_5564cd.jpg' here to replace this placeholder)*

## ✨ Key Features

### 🛒 Interactive Shopping Experience
* [cite_start]**AJAX-Powered Cart:** Add, remove, and update items in real-time without refreshing the page[cite: 131, 140].
* [cite_start]**Offcanvas Sidebar:** A modern slide-out cart interface built with Bootstrap 5 Offcanvas[cite: 119].
* [cite_start]**Real-Time Feedback:** Integrated **Toastr.js** for instant success/error notifications (e.g., "Item added to cart")[cite: 122, 134].

### 🔐 Security & User Management
* [cite_start]**Role-Based Access:** Distinct interfaces for **Guests**, **Customers**, and **Admins**[cite: 106].
* [cite_start]**Guest Lock:** Unique "Login Required" interface in the cart sidebar for non-authenticated visitors, guiding them to sign up[cite: 120].
* **Secure Authentication:** Powered by **ASP.NET Identity** with CSRF protection on all sensitive actions.

### 📦 Inventory & Data Logic
* **Smart Stock Management:** prevents users from adding more items than available in stock. Displays warnings like *"Maximum stock reached"* when limits are hit.
* [cite_start]**Dynamic Search:** Filter books by Title, Author, or ISBN via the navigation bar[cite: 98].

## 🛠️ Tech Stack

* **Backend:** C# ASP.NET MVC 5 (.NET Framework)
* **Database:** MS SQL Server & Entity Framework 6 (Code First)
* **Frontend:** HTML5, Razor View Engine, Bootstrap 5.3
* **Scripting:** jQuery, AJAX, Toastr.js (Notifications)
* **Tools:** Visual Studio, NuGet Package Manager

## 📸 Screenshots

| **Home & Search** | **Shopping Cart Sidebar** |
|:---:|:---:|
| ![Home](path/to/image_5564cd.jpg) | ![Cart](path/to/image_56c90d.jpg) |
| *Responsive product grid with search* | *AJAX cart with stock validation* |

| **Guest Experience** | **Login Prompt** |
|:---:|:---:|
| ![Guest Lock](path/to/image_54ef5c.png) | *Sidebar prompts guests to login* |

## 🚀 How to Run

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/ShengYong04/GoBooks.git](https://github.com/ShengYong04/GoBooks.git)
    ```
2.  **Open in Visual Studio**
    * Open the `.sln` solution file.
3.  **Setup Database**
    * Open **Package Manager Console** (`Tools > NuGet Package Manager > Package Manager Console`).
    * Run the command: `Update-Database` to create the local SQL database and seed initial data.
4.  **Run the Application**
    * Press `F5` to start the server.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).
