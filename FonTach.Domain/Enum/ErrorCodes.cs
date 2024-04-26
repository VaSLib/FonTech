namespace FonTech.Domain.Enum;

public enum ErrorCodes
{
    // 0-10 Reports
    // 11-20 Users

    ReportsNotFound = 0,
    ReportNotFound = 1,
    ReportAlreadyExists = 2,

    InternalServerError = 10,

    UserNotFound = 11,
    UserAlreadyExists = 12,
    UserUnauthorizedAccess = 13,
    UserAlreadyExistsThisRole = 14,

    PasswordNotEqualsPasswordConfirm = 21,
    PasswordIsWrong = 22,

    RoleAlreadyExists = 31,
    RoleNotFound = 32,
}
