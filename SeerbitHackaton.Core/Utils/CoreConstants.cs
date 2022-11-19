
using SeerbitHackaton.Core.Enums;

namespace SeerbitHackaton.Core.Utils
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";

        public static readonly string[] validExcels = new[] { ".xls", ".xlsx" };

        public const string TestPdfTemplatePath1 = @"filestore/pdftemplate/TestPdfTemplate1.html";
        public const string TestPdfTemplatePath2 = @"filestore/pdftemplate/TestPdfTemplate2.html";
        public const string ResultPdfTemplatePath = @"pdftemplate/ResultTemplate.html";


        public static class EntityType
        {
            public const string School = nameof(School);
            public const string User = nameof(User);
        }

        public class EmailTemplate
        {
            public EmailTemplate(EmailType emailType, string subject, string templatePath)
            {
                Subject = subject;
                TemplatePath = templatePath;
                EmailType = emailType;
            }
            public EmailType EmailType { get; set; }
            public string Subject { get; set; }
            public string TemplatePath { get; set; }
        }

        public static readonly Dictionary<EmailType, EmailTemplate> EmailTemplates = new Dictionary<EmailType, EmailTemplate>
        {
            { EmailType.UserRegistration, new EmailTemplate(EmailType.UserRegistration, "User Registration", "user-registration.html") },
            { EmailType.PasswordReset, new EmailTemplate(EmailType.PasswordReset, "Password Reset Request", "password-reset.html") },
            { EmailType.SuccessPasswordReset, new EmailTemplate(EmailType.SuccessPasswordReset, "Successful Password Reset", "success-password-reset.html") },
            { EmailType.TwoFactorAuthentication, new EmailTemplate(EmailType.TwoFactorAuthentication, "Two Factor Authentication Code", "two-factor-auth.html") },
            { EmailType.EmailVerificationCode, new EmailTemplate(EmailType.EmailVerificationCode, "Email Verification Code", "email-verification-code.html") },
            { EmailType.SuccessEmailVerification, new EmailTemplate(EmailType.SuccessEmailVerification, "Email Verification Success", "email-verification-success.html") },
             { EmailType.PasswordCreation, new EmailTemplate(EmailType.PasswordCreation, "Password Creation", "password-creation.html") },
        };

        public class PaginationConsts
        {
            public const int PageSize = 25;
            public const int PageIndex = 1;
        }

        public class ClaimsKey
        {
            public const string LastLogin = nameof(LastLogin);
            public const string TenantId = nameof(TenantId);
            public const string UserType = nameof(UserType);
            public const string Institution = nameof(Institution);
            public const string Organization = nameof(Organization);
            public const string Permissions = nameof(Permissions);
            public const string InstitutionId = nameof(InstitutionId);
            public const string GraduateId = nameof(GraduateId);

        }

        public class AllowedFileExtensions
        {
            public const string Signature = ".jpg,.png";
        }

        public class Dashboard
        {

            public static string[] Months = new string[] {
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
        }

    }
}