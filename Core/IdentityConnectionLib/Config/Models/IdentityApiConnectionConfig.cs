using Core.HttpLogic.Base;

namespace IdentityConnectionLib.Config.Models;

public class IdentityApiConnectionConfig : IIdentityApiConnectionConfig
{
    public ConnectionType ConnectionType => ConnectionType.Http;
    public string ProfilesInfoUri => "https://localhost:7016/api/users/profiles";
    public string UsersInfoUri => "https://localhost:7016/api/users/list";
}