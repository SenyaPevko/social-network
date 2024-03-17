namespace Core.Logic.Connections.HttpLogic.HttpResponses.Models
{
    public record HttpResponse<TResponse> : BaseHttpResponse
    {
        /// <summary>
        ///     Response body
        /// </summary>
        public TResponse Body { get; set; }
    }
}