using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.Event
{
    public interface IEvent<T>
    {
        Task Handle(T @event);
    }
}
