namespace AuthGuard.BLL.Domain.Entities
{
    public enum Template
    {
        CustomerMessage = 1,
        AccountConfirmation = 2,
        ResetPassword = 3,
        TwoFactorCode = 4,
        PasswordlessSignUp = 5,
        PasswordlessLogIn = 6,
        AddLocalProvider = 7
    }
}