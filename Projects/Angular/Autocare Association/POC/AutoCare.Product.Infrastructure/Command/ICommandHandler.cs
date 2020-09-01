using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Command
{
    public interface ICommandHandler<in T>
        where T: ICommand
    {
        Task Handle(T command);
    }
}
