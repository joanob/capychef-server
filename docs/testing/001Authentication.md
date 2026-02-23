# Authentication test cases

## Signup
- [ ] Create a new user with username, email and password
- [ ] Create a new user with username and password 
- [ ] Create a guest user
- [ ] Create user with empty username should fail
- [ ] Create a user with an existing username should fail
- [ ] Create user with invalid email should fail
- [ ] Create a user with an existing email should fail

## Login 
- [ ] Login with valid username and password
- [ ] Login with valid email and password
- [ ] Login with invalid username should fail
- [ ] Login with invalid email should fail
- [ ] Login with invalid password should fail
- [ ] Login to a blocked user should fail
- [ ] Login to a deleted user should fail

## Recover password 
- [ ] Request password reset with valid email
- [ ] Request password reset with invalid email should fail
- [ ] Request password reset with non-existent email should fail
- [ ] Request password reset with blocked user should fail
- [ ] Request password reset with deleted user should fail

## Reset password
- [ ] Reset password with valid token and new password should revoke user sessions
- [ ] Reset password with invalid token should fail
- [ ] Reset password with expired token should fail
- [ ] Reset password with blocked user should fail
- [ ] Reset password with empty token should fail
- [ ] Reset password with valid token and empty password should fail
- [ ] Reset password with valid token but blocked user should fail
- [ ] Reset password with valid token but deleted user should fail

## Change password
- [ ] Change password with valid current password and new password should revoke user sessions but not the current session
- [ ] Change password with invalid current password should fail
- [ ] Change password with blocked user should fail
- [ ] Change password with empty current password should fail
- [ ] Change password with valid current password and empty new password should fail
- [ ] Change password with valid current password but blocked user should fail
- [ ] Change password with valid current password but deleted user should fail

## Guest transference
- [ ] Create guest transfer token with guest user
- [ ] Create guest transfer token with non guest user should fail
- [ ] Create guest transfer token with unauthenticated user should fail
- [ ] Create guest transfer token with blocked guest user should fail
- [ ] Create guest transfer token with deleted guest user should fail

## Guest login 
- [ ] Guest login with valid token
- [ ] Guest login with invalid token should fail
- [ ] Guest login with blocked user should fail
- [ ] Guest login with valid token but blocked user should fail
- [ ] Guest login with valid token but deleted user should fail

## Get auth session 
- [ ] Get auth session with household
- [ ] Get auth session without household
- [ ] JWT should be valid after signup
- [ ] JWT should be valid after login
- [ ] JWT should be invalid after logout
- [ ] JWT should refresh session token
- [ ] JWT refresh should be invalid after password change
- [ ] JWT refresh should be invalid after password reset
- [ ] JWT refresh should be invalid after user deletion
- [ ] JWT refresh should be invalid after user blocking

## Logout
- [ ] Logout should invalidate JWT and revoke sessions