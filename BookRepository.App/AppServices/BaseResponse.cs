using System.Net;

namespace BookRepository.App.AppServices
{
    public class BaseResponse
    {
        //Todo: In general it is a bad practice to assume everything is ok. 
        //Since switching on result code in the controller breaks separation of concerns, this property is used until a proper middleware is in place 
        public HttpStatusCode Result { get; set; } = HttpStatusCode.OK;
    }
}
