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
    PasswordIsWrong = 13,

    PasswordNotEqualsPasswordConfirm = 21,
}
