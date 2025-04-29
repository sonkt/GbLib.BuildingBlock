using MediatR;

namespace GbLib.BuildingBlock.Application.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}