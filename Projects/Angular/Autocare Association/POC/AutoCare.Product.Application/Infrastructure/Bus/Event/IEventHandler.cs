namespace AutoCare.Product.Application.Infrastructure.Bus.Event
{
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand>
        where TCommand: class, Bus.Command.ICommand, IMessage
    {
    }
}
