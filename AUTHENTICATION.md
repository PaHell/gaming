# Authentication Implementation

This document describes the authentication implementation in the gaming application.

## Overview

Authentication uses JWT tokens stored in HTTP-only cookies for security. The implementation includes:
- Login and registration endpoints
- Automatic cookie handling (no manual cookie management needed on frontend)
- Comprehensive logging for debugging
- Proper redirect handling after authentication

## Backend Implementation

### Endpoints

Located in `backend/Controllers/Identity/AuthenticationController.cs`:

- `POST /register` - Create new user account
- `POST /login` - Authenticate existing user
- `POST /logout` - Revoke user session (requires authentication)

### Cookie Configuration

Cookies are configured in `backend/Services/Identity/UserService.cs` with secure defaults:

```csharp
HttpOnly = true        // Prevents JavaScript access (XSS protection)
Secure = true          // HTTPS only
SameSite = Strict      // CSRF protection
Domain = "localhost"   // Configured in appsettings.json
Path = "/"             // Available to all routes
```

Two cookies are set on successful authentication:
- `access_token` - JWT token for API authentication (expires in 120 seconds)
- `refresh_token` - Token for session renewal (expires in 240 seconds)

### Logging

All authentication operations log the following:
- Login attempts (success/failure with email)
- Registration attempts (success with email and session ID)
- Logout attempts (success/failure with session ID)
- Sign-in/sign-out operations in UserService

This logging helps debug authentication issues without exposing sensitive data.

## Frontend Implementation

### API Client

Located in `frontend/src/lib/clients.ts`:

The `AuthenticationClient` class provides three methods:
- `login(form: LoginForm)` - Returns UserSession on success
- `register(form: RegisterForm)` - Returns UserSession on success  
- `logout()` - Returns revoked UserSession on success

### Automatic Cookie Handling

The `ClientBase` class automatically includes credentials with all requests:

```typescript
protected async transformOptions(options: RequestInit): Promise<RequestInit> {
    options.credentials = "include";  // Sends cookies automatically
    return options;
}
```

**Important**: This means:
- ✅ Cookies are sent automatically with every API request
- ✅ No need to manually extract or pass tokens in frontend code
- ✅ No need to manage cookie storage - browser handles it
- ✅ Works seamlessly with SSR and CSR

### User Interface

#### Login Page
- Location: `frontend/src/routes/auth/login/+page.svelte`
- Component: `frontend/src/lib/shadcn/components/login-form.svelte`
- Features:
  - Email and password inputs
  - Loading state during authentication
  - Error messages via toast notifications
  - Automatic redirect to `/app/dashboard` on success
  - Client-side logging for debugging

#### Register Page
- Location: `frontend/src/routes/auth/register/+page.svelte`
- Component: `frontend/src/lib/shadcn/components/register-form.svelte`
- Features:
  - First name, last name, email, and password inputs
  - Loading state during registration
  - Error messages via toast notifications
  - Automatic redirect to `/app/dashboard` on success
  - Client-side logging for debugging

### Toast Notifications

Success and error messages are displayed using `svelte-sonner`:

- Success: "Login successful! Redirecting..." / "Registration successful! Redirecting..."
- Error: "Invalid email or password" (401) / "Login failed. Please try again." (other errors)

The `Toaster` component is added to the root layout (`frontend/src/routes/+layout.svelte`) to display notifications application-wide.

## Security Considerations

1. **HTTP-Only Cookies**: Tokens cannot be accessed by JavaScript, preventing XSS attacks
2. **Secure Flag**: Cookies only sent over HTTPS (in production)
3. **SameSite=Strict**: Prevents CSRF attacks
4. **Short Expiration**: Access tokens expire in 2 minutes, refresh tokens in 4 minutes
5. **Password Hashing**: Uses ASP.NET Identity's PasswordHasher (PBKDF2)
6. **Session Revocation**: Sessions can be explicitly revoked on logout

## Development Notes

### Environment Variables

Frontend requires `PUBLIC_BACKEND_URL_CLIENT` environment variable pointing to the backend API (e.g., `http://localhost:3002`).

### Testing Authentication

1. Start backend: `cd backend && dotnet run`
2. Start frontend: `cd frontend && npm run dev`
3. Navigate to http://localhost:3000/auth/register
4. Register a new account - should redirect to dashboard
5. Logout (when implemented in UI)
6. Login at http://localhost:3000/auth/login - should redirect to dashboard

### Debugging

Check browser console and backend logs for authentication flow:
- Frontend logs include: "Login attempt", "Login successful", "Registration attempt", etc.
- Backend logs include: Email addresses, session IDs, success/failure status

## Future Enhancements

- [ ] Add logout button to UI
- [ ] Implement session refresh logic
- [ ] Add "Remember me" functionality
- [ ] Implement password reset flow
- [ ] Add email verification
- [ ] Create protected route guards
- [ ] Add session state management (Svelte store)
