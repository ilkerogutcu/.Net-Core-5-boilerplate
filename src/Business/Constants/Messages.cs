namespace Business.Constants
{
    /// <summary>
    /// Messages for responses
    /// </summary>
    public static class Messages
    {
        public const string UsernameAlreadyExist = "Username already exist!";
        public const string EmailAlreadyExist = "Username already exist!";
        public const string SignInSuccessfully = "Sign in successfully.";
        public const string RequiredTwoFactoryCode = "Required two factory code!";
        public const string SignInFailed = "Sign in failed!";
        public const string UserNotFound = "User not found!";
        public const string PasswordsDontMatch = "Passwords don't match!";
        public const string PleaseEnterTheEmail = "Please enter the email addres!";
        public const string PleaseEnterAValidEmail = "Please enter a valid email address";
        public const string PleaseEnterTheFirstName = "Please enter the first name!";
        public const string PleaseEnterTheLastName = "Please enter the last name!";
        public const string SignUpFailed = "Sign up failed!";
        public const string ForgotPasswordFailed = "Forgot password failed!";
        public const string SentForgotPasswordEmailSuccessfully = "Sent forgot password email successfully.";
        public const string EmailIsNotConfirmed = "Email is not confirmed!";
        public const string SignUpSuccessfully = "Sign up successfully. Please confirm your account by visiting this URL: ";
        public const string TokenCreatedSuccessfully = "Token created successfully.";
        public const string FailedToCreateToken = "Failed to create token!";
        public const string EmailSuccessfullyConfirmed = "Email successfully confirmed.";
        public const string ErrorVerifyingMail = "There was an error verifying email!";
        public const string PasswordHasBeenResetSuccessfully = "Your password has been reset successfully.";
        public const string PasswordResetFailed = "Error occured while reseting the password!";
        public const string FailedToUpdateUser = "Failed to update user!";
        public const string UpdatedUserSuccessfully = "User updated succesfully.";
        public const string GetDateRangeError = "End date must be greater than or equal to start date!";
    }
}