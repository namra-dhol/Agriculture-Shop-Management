# 🔐 JWT Authentication Implementation Guide

## Overview

I've successfully implemented a complete JWT-based authentication system for your Agriculture Shop Management application. This includes user registration, login, and role-based access control.

## 🚀 Features Implemented

### ✅ **Backend API (AgriShop)**
- **JWT Token Generation & Validation**
- **User Registration** with role selection (Admin/Customer)
- **User Login** with username/password
- **Password Hashing** using BCrypt
- **Role-based Authorization** for API endpoints
- **Token-based Authentication** for protected routes

### ✅ **Frontend (AgriShop_Consume)**
- **Login Page** with form validation
- **Registration Page** with role selection
- **Dynamic Navigation** showing login/logout based on auth state
- **Token Storage** in localStorage
- **User Profile Display** in header

## 📋 API Endpoints

### Authentication Endpoints

| Endpoint | Method | Description | Access |
|----------|--------|-------------|---------|
| `/api/auth/register` | POST | Register new user | Public |
| `/api/auth/login` | POST | Login user | Public |
| `/api/auth/me` | GET | Get current user info | Authenticated |

### Protected Endpoints

| Endpoint | Method | Description | Access |
|----------|--------|-------------|---------|
| `/api/users` | GET | List all users | Admin only |
| `/api/users/{id}` | GET | Get user by ID | Self or Admin |
| `/api/users` | POST | Create user | Admin only |
| `/api/users/{id}` | PUT | Update user | Self or Admin |
| `/api/users/{id}` | DELETE | Delete user | Admin only |

## 🔧 Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong",
    "Issuer": "AgriShop",
    "Audience": "AgriShopUsers",
    "ExpirationInMinutes": 60
  }
}
```

### Required NuGet Packages
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `System.IdentityModel.Tokens.Jwt`
- `BCrypt.Net-Next`

## 🎯 How to Use

### 1. **User Registration**
```json
POST /api/auth/register
{
  "userName": "john_doe",
  "email": "john@example.com",
  "password": "password123",
  "address": "123 Main St, City",
  "phone": "+1234567890",
  "role": "Customer"  // or "Admin"
}
```

### 2. **User Login**
```json
POST /api/auth/login
{
  "userName": "john_doe",
  "password": "password123"
}
```

### 3. **Using Protected Endpoints**
Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

## 🛡️ Security Features

### **Password Security**
- Passwords are hashed using BCrypt
- Minimum 6 characters required
- Secure password verification

### **Token Security**
- JWT tokens with 60-minute expiration
- Secure key signing
- Role-based claims in tokens

### **Authorization**
- Role-based access control (Admin/Customer)
- Users can only access their own data
- Admins have full access to all data

## 🎨 Frontend Features

### **Dynamic UI**
- Login/Register buttons when not authenticated
- User dropdown with profile/logout when authenticated
- Role display in user menu

### **Token Management**
- Automatic token storage in localStorage
- Token-based API calls
- Automatic logout on token expiration

## 🧪 Testing

### **Test Endpoints**
- `/api/test/public` - No authentication required
- `/api/test/protected` - Authentication required
- `/api/test/admin` - Admin role required
- `/api/test/customer` - Customer role required

## 📝 Usage Examples

### **Register a New Customer**
1. Go to `/Home/Register`
2. Fill in the form
3. Select "Customer" role
4. Submit to create account

### **Register a New Admin**
1. Go to `/Home/Register`
2. Fill in the form
3. Select "Admin" role
4. Submit to create account

### **Login**
1. Go to `/Home/Login`
2. Enter username and password
3. Submit to get JWT token

### **Access Protected Resources**
- The frontend automatically includes the JWT token in API calls
- Protected pages will redirect to login if not authenticated

## 🔄 Next Steps

### **Recommended Enhancements**
1. **Token Refresh** - Implement refresh token mechanism
2. **Password Reset** - Add forgot password functionality
3. **Email Verification** - Verify email addresses
4. **Profile Management** - User profile editing
5. **Session Management** - Better session handling
6. **Audit Logging** - Track user actions

### **Security Improvements**
1. **Rate Limiting** - Prevent brute force attacks
2. **Two-Factor Authentication** - Additional security layer
3. **Account Lockout** - Lock accounts after failed attempts
4. **HTTPS Enforcement** - Secure all communications

## 🚨 Important Notes

1. **Change the JWT Secret Key** in production
2. **Use HTTPS** in production environments
3. **Implement proper error handling** for token expiration
4. **Add input validation** on all forms
5. **Consider implementing refresh tokens** for better UX

## 📞 Support

If you encounter any issues:
1. Check the browser console for JavaScript errors
2. Verify the API endpoints are accessible
3. Ensure the database connection is working
4. Check that all NuGet packages are installed

---

**🎉 Your authentication system is now ready to use!** Users can register, login, and access role-based features with secure JWT tokens. 