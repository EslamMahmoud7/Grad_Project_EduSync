using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(string ToEmail, string subject, string body);
    }
}
