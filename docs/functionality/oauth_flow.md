﻿## Overview

The Link Admin BFF (Backend for Frontend) implements a secure authentication flow using OAuth 2.0 with cookie-based session management. This provides a robust security model where the BFF acts as the OAuth client and manages user sessions through secure HTTP-only cookies.

## Authentication Flow

1. **Initial Login Request**

    - The UI redirects users to `/api/login` on the BFF
    - The BFF initiates the OAuth authorization code flow by redirecting to the configured OAuth provider
    - The OAuth provider authenticates the user and returns an authorization code

2. **Token Exchange & Session Creation**

    - The BFF exchanges the authorization code for access/refresh tokens
    - The BFF creates a session and issues a secure HTTP-only cookie named "link_cookie"
    - The cookie contains session information but not the actual OAuth tokens
    - OAuth tokens are securely stored server-side and managed by the BFF

3. **Subsequent Requests**

    - The UI includes the session cookie automatically with each request
    - The BFF validates the cookie and retrieves the associated session
    - The BFF uses the OAuth tokens to make authenticated requests to backend services
    - Token refresh is handled transparently by the BFF when needed

## Cookie Security

- Cookies are HTTP-only to prevent XSS attacks
- Secure flag ensures cookies only sent over HTTPS
- Anti-forgery protection enabled
- Session cookies with server-side storage

## Token Security

- OAuth tokens never exposed to the frontend
- Tokens stored securely server-side
- Token refresh handled by BFF
- Short-lived access tokens with automatic refresh

## CORS Protection

- Strict CORS policy configured
- Credentials mode enabled for cookie transmission
- Only trusted origins allowed

## PKCE (Proof Key for Code Exchange)

PKCE is implemented to secure the OAuth authorization code flow and prevent authorization code interception attacks:

**Code Verifier Generation**

   - A cryptographically random code verifier is generated for each authentication request
   - The verifier is stored securely server-side in the BFF
   - Length and character requirements follow OAuth PKCE specifications

**Code Challenge**

   - The code challenge is derived from the verifier using SHA-256
   - Challenge is included in the initial authorization request
   - The challenge method is set to "S256" (SHA-256)

**Code Exchange**

   - Original code verifier is included when exchanging the authorization code
   - OAuth provider validates the code challenge/verifier pair
   - Prevents malicious actors from using intercepted auth codes

## State Parameter Protection

The state parameter provides CSRF protection during the OAuth flow:

**State Generation**

   - Cryptographically secure random state value generated per request
   - State is bound to the user's session
   - Stored server-side in the BFF

**State Validation**

   - State parameter included in authorization request
   - Returned state must match the original stored value
   - Requests with invalid/missing state are rejected
   - Protects against CSRF and replay attacks

## Scope Restrictions

OAuth scopes are strictly controlled to limit access:

**Required Scopes**

openid profile email

**Scope Validation**

- Only pre-configured scopes are allowed
- Scope requests are validated against allowed list
- Additional scopes require explicit configuration
- Prevents scope elevation attacks

**Scope Usage**

- Scopes map to specific API permissions
- Access tokens only receive approved scopes
- Resource access is restricted by granted scopes

## Configuration

The BFF's OAuth and cookie settings are configured in `appsettings.json`:

```json
"Authentication": {
  "DefaultScheme": "link_cookie",
  "DefaultChallengeScheme": "link_oauth2",
  "Schemas": {
    "Cookie": {
      "HttpOnly": true,
      "Domain": "",
      "Path": "/"
    },
    "Oauth2": {
      "Enabled": true,
      "ClientId": "",
      "ClientSecret": "",
      "Endpoints": {
        "Authorization": "",
        "Token": "",
        "UserInformation": ""
      },
      "CallbackPath": "/api/signin-oauth2"
    }
  }
}
```

## UI Integration

The UI application interacts with the BFF's authentication system through:

### Login

```javascript
// Redirect to BFF login endpoint
window.location.href = '/api/login';
```

### Session Status

```javascript
// Check if user is authenticated
fetch('/api/user', {
credentials: 'include' // Important for cookie transmission
});
```

### Logout

```javascript
// Redirect to BFF logout endpoint
    window.location.href = '/api/logout';
```

---

The UI never directly handles OAuth tokens or credentials - all authentication is delegated to and managed by the BFF.