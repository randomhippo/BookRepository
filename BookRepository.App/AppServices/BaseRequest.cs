using MediatR;

namespace BookRepository.App.AppServices
{
    public class BaseRequest<T> : IRequest<T> where T : BaseResponse
    {
    }
}
