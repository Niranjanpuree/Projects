namespace AutoCare.Product.Application.Infrastructure.Bus.Command
{
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand>
        where TCommand: class, ICommand, IMessage
    {
    }
}
