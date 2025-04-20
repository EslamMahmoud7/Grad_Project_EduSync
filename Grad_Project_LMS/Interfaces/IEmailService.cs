namespace Grad_Project_LMS.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string ToEmail, string subject, string body);
    }
}
