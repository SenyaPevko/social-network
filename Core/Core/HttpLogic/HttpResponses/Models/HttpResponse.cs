namespace Core.HttpLogic.HttpResponses.Models;

public record HttpResponse<TResponse> : BaseHttpResponse
{
    /// <summary>
    ///     Response body
    /// </summary>
    public TResponse Body { get; set; }
}