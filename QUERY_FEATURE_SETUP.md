# Product Query Feature Setup Guide

## Overview
This feature allows users to send product queries directly from the ProductDetail page. When a user selects a variant and clicks "Send Query", an email is sent with the product details.

## Features Implemented

### Backend (AgriShop API)
1. **QueryDTO** - Data transfer object for query requests
2. **QueryController** - API endpoint for sending queries using your working email code
3. **MailKit Integration** - Direct SMTP email sending

### Frontend (AgriShop_Consume MVC)
1. **ProductDetail Integration** - Query functionality integrated into existing product detail page
2. **Query Models** - Models for request/response handling
3. **JavaScript Integration** - Dynamic query sending from product detail page

## How It Works

1. **User visits ProductDetail page** (e.g., `/Product/ProductDetail/1`)
2. **User selects a variant** from the available options
3. **"Send Query" button becomes enabled**
4. **User clicks "Send Query"** 
5. **Email is sent** with product details to your configured email
6. **Success message is shown** and form resets

## Email Configuration

The email is sent using your working SMTP configuration:
- **SMTP Server**: smtp.gmail.com
- **Port**: 587
- **Email**: nd.demo06@gmail.com
- **App Password**: kbzn ezmn sela kuut

## Email Format

The email includes:
- **Subject**: "Query about Product Purchase"
- **Predefined content**: Professional email template
- **Dynamic content**: Product type, name, variant, and price
- **Customer message**: Predefined interest message

## API Endpoints

- `POST /api/Query/send` - Send query email
- `GET /api/Query/test` - Test endpoint for debugging

## Testing

### 1. Start Both Applications
```bash
# Terminal 1 - API
cd AgriShop
dotnet run

# Terminal 2 - MVC
cd AgriShop_Consume
dotnet run
```

### 2. Test the API
- Visit: `http://localhost:5275/api/Query/test`
- Should show: `{"message":"Query API is working!","timestamp":"..."}`

### 3. Test the Complete Flow
- Go to any product detail page (e.g., `/Product/ProductDetail/1`)
- Select a variant
- Click "Send Query"
- Check your email (`nd.demo06@gmail.com`) for the query

## Troubleshooting

### Email Not Sending
- Verify Gmail App Password is correct
- Check if Gmail 2FA is enabled
- Ensure firewall allows SMTP connections

### API Errors
- Check if both applications are running
- Verify API endpoints are accessible
- Check browser console for errors

### Database Issues
- Ensure all required tables exist
- Verify relationships between tables
- Check if sample data is present

## Files Modified

### Backend
- `AgriShop/Controllers/QueryController.cs` - Updated with working email code
- `AgriShop/Program.cs` - Removed email service dependencies
- `AgriShop/appsettings.json` - Removed email settings

### Frontend
- `AgriShop_Consume/Views/Product/ProductDetail.cshtml` - Added query functionality
- `AgriShop_Consume/Controllers/QueryController.cs` - API consumption
- `AgriShop_Consume/Models/QueryModels.cs` - Request/response models

## Removed Files
- `AgriShop/Services/EmailService.cs` - No longer needed
- `AgriShop/Models/EmailSettings.cs` - No longer needed
- `AgriShop_Consume/Views/Query/Index.cshtml` - Separate page removed

The implementation now uses your working email code and integrates seamlessly with the existing product detail page!
