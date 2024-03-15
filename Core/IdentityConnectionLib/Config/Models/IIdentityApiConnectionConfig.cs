using Core.Logic.Connections.Base;

namespace IdentityConnectionLib.Config.Models
{
    public interface IIdentityApiConnectionConfig : IConnectionConfiguration
    {
        string ProfilesInfoUri { get; }
        string UsersInfoUri { get; }
    }
}
