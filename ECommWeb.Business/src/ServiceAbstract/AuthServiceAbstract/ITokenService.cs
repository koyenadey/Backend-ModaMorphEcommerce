using ECommWeb.Core.src.Entity;

namespace ECommWeb.Business.src.ServiceAbstract.AuthServiceAbstract;

public interface ITokenService
{
    public string GetToken(User user);

}
