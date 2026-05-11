# 🛍️ UniWear — AI-Powered Fashion E-Commerce Web Application

🚀 **UniWear** is a modern full-stack fashion e-commerce platform built using ASP.NET Core MVC, designed to deliver a seamless online shopping experience with secure authentication, smart outfit recommendations, cart management, and online payments.

---

## ✨ Key Features

🔐 **Secure Authentication**
- ASP.NET Identity (Email & Password)
- Google OAuth 2.0 Login
- Secure session & user management

🛒 **E-Commerce Functionalities**
- Browse products by category (Men, Women, Kids)
- Product details with images & descriptions
- Add to Cart with live cart count update (AJAX)
- Wishlist / Favorites system
- Dynamic cart quantity management
- Stock availability validation
- Automatic stock reduction after order placement


🤖 **Smart Outfit Recommendation System**
- AI-powered outfit recommendation integration using Gemini API
- Rule-based intelligent outfit matching
- Product matching based on:
- Gender
- Occasion
- Color combinations
- Clothing type


💳 **Online Payments**
- Stripe Payment Integration (Test Mode)
- Secure checkout flow
- Payment success-based order creation

📦 **Order Management**
- User-specific order history
- My Orders page
- Order summary with purchased products
- Order status tracking

⚡ **Interactive User Experience**
- AJAX-based cart updates without page refresh
- Responsive UI using Bootstrap 5
- Modern shopping experience
  
---

## 🛠️ Tech Stack

| Technology | Description |
|----------|------------|
| ASP.NET Core MVC | Backend Framework |
| Entity Framework Core | ORM for database |
| SQL Server | Database |
| Bootstrap 5 | Frontend styling |
| Stripe API | Payment Gateway |
| ASP.NET Identity | Authentication |
| Gemini API | AI Outfit Recommendations |
| JavaScript & AJAX	| Dynamic interactions |


---

## 🔐 Authentication System

- User registration & login
- Google Sign-In integration
- Secure password hashing
- Session management
- Role-based Identity System

---

## 💳 Payment Integration

- Stripe Checkout (Test Mode)
- Simulated transactions
- Easy integration for production upgrade


🧠 **Advanced Features**
AI-based fashion recommendation engine
Smart outfit generation
Dynamic stock management
Live cart updates
Secure payment integration

---

## 📸 Application Screenshots

### 🏠 Home Page
![Screenshot_8-4-2026_135410_localhost](https://github.com/user-attachments/assets/84d63171-ac5f-4d6a-9936-d0ddb68d8d7c)


### 🔐 Login with Google
![Screenshot_8-4-2026_135512_localhost](https://github.com/user-attachments/assets/6f793b84-7b87-446f-be1d-395ffb4ac54b)


### Product Details
<img width="1875" height="1039" alt="Screenshot_11-5-2026_113238_localhost" src="https://github.com/user-attachments/assets/8123b926-4c2d-4bab-929e-a32e840c1a18" />


### AI Recommendation System
<img width="1863" height="904" alt="Screenshot_11-5-2026_113215_localhost" src="https://github.com/user-attachments/assets/d9bc2026-0818-4563-a726-3f91e33d31f5" />


### 🛒 Cart Page
<img width="1876" height="906" alt="Screenshot_11-5-2026_113657_localhost" src="https://github.com/user-attachments/assets/8d6e7f48-5d42-4ac9-aca7-8bc65189f4c4" />




### Stripe Payment
![Screenshot_8-4-2026_13573_checkout stripe com](https://github.com/user-attachments/assets/0cfc1596-73c5-4a28-8a91-2e33f4c7a806)

<img width="1870" height="947" alt="Screenshot_11-5-2026_113759_localhost" src="https://github.com/user-attachments/assets/e83a4609-c86c-4312-9703-3147d15d4bda" />



---
## ⚙️ Setup Instructions

### 1️⃣ Clone the repository
```bash
git clone https://github.com/Sukhwinder42/uniwear.git
cd uniwear

Open the .sln file in Visual Studio
Restore NuGet packages (automatic)

Update-Database

dotnet run


