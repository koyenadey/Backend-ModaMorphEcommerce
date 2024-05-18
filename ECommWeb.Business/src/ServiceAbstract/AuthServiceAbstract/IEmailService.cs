using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

public interface IEmailService
{
    void SendEmail(string email, string subject, string body);
}
