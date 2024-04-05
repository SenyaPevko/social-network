using Core.Logic.Connections.Base;
using Core.Logic.Connections.RabbitMqLogic.Connections.Models;

namespace IdentityConnectionLib.Config.Models;

public class IdentityApiConnectionConfig : IIdentityApiConnectionConfig
{
    public ConnectionType ConnectionType => ConnectionType.RabitMq;
    public string ProfilesInfoUri => "https://localhost:7016/api/users/profiles";
    public string UsersInfoUri => "https://localhost:7016/api/users/list";

    public string RabbitMqUrl => "amqps://pshadjjf:yJPxBF9bl-nUY2vIBirOpdp540dIw-3X@whale.rmq.cloudamqp.com/pshadjjf";

    public RabbitMqConnectionType RabbitMqConnectionType => RabbitMqConnectionType.Uri;

    public string RabbitMqHostName => "localhost";
}