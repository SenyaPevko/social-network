using Core.HttpLogic.Base;

namespace IdentityConnectionLib.Config.Models
{
    public interface IIdentityApiConnectionConfig
    {
        ConnectionType ConnectionType { get; }
        string ProfilesInfoUri { get; }
        string UsersInfoUri { get; }
    }
}
