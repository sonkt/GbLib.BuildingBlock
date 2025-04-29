using MediatR;

namespace GbLib.BuildingBlock.Application.CQRS;

public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public abstract Task<TResult> Handle(TQuery request, CancellationToken cancellationToken);
}