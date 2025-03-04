namespace API.Enums;

public enum AuthStatus
{
    NotLoggedIn = 0,
    LoggedIn = 1,
    EmailConfirmationSent = 2,
    EmailConfirmed = 3,
    EmailAlreadyConfirmed = 4,
    InvalidConfirmationToken = 5,
    TwoFactorRequired = 6,
    InvalidTwoFactorCode = 7,
    LockedOut = 8,
    InvalidCredentials = 9
}
