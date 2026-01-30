namespace Infrastucture.Services.EmailService;

public static class EmailTemplates
{
    public static string GetOtpTemplate(string otpCode) => $@"
        <div style='font-family: sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee;'>
            <h2 style='color: #3880ff;'>Cinemax Verification</h2>
            <p>Your verification code is:</p>
            <div style='background: #f4f4f4; padding: 15px; text-align: center; font-size: 32px; font-weight: bold; letter-spacing: 5px;'>
                {otpCode}
            </div>
            <p style='color: #666; font-size: 14px; margin-top: 20px;'>
                This code will expire in 15 minutes. If you didn't request this, please ignore this email.
            </p>
        </div>";

    public static string GetWelcomeTemplate(string name = "")
    {
        var formattedName = string.IsNullOrEmpty(name) ? "" : ", " + name;
        
        return $@"
        <h1>Welcome{formattedName}!</h1>
        <p>Glad to have you with us. Explore the latest movies now!</p>";
    }
}