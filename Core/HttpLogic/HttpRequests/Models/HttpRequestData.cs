using Core.HttpLogic.HttpRequests.Services;

namespace Core.HttpLogic.HttpRequests.Models
{
    public record HttpRequestData
    {
        /// <summary>
        /// Method type
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Rwquest address
        /// </summary>\
        public Uri Uri { set; get; }

        /// <summary>
        /// Method body
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// Content-type in the request
        /// </summary>
        public ContentType ContentType { get; set; } = ContentType.ApplicationJson;

        /// <summary>
        /// Request headers
        /// </summary>
        public IDictionary<string, string> HeaderDictionary { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Collection of query parameters
        /// </summary>
        public ICollection<KeyValuePair<string, string>> QueryParameterList { get; set; } =
            new List<KeyValuePair<string, string>>();
    }
}
