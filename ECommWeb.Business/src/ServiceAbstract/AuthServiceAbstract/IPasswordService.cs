using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

public interface IPasswordService
{
    string HashPassword(string password, out byte[] salt);
    bool VerifyPassword(string password, string passwordHash, byte[] salt);
}
