using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Command
{
    public interface ICommandBus
    {
        Task RegisterAsync<T, THandler>(T command, THandler commandHandler)
            where T : class, ICommand
            where THandler : class, ICommandHandler<T>;
        Task SendAsync<T>(T command) where T : class, ICommand;
    }
}