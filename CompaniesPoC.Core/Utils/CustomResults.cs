using System.Net;

namespace CompaniesPoC.Core.Utils
{
    public class CustomResults<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
