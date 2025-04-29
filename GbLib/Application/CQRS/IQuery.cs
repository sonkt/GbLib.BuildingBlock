using MediatR;

namespace GbLib.Application.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}