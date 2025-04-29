using MediatR;

namespace GbLib.BuildingBlock.Application.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}