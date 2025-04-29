using MediatR;

namespace GbLib.BuildingBlock.Application.CQRS;

public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public abstract Task<TResult> Handle(TCommand request, CancellationToken cancellationToken);
}