using MediatR;

namespace GbLib.Application.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}