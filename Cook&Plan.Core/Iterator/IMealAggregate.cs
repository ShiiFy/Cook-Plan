using Cook_Plan.Core.Composite;

namespace Cook_Plan.Core.Iterator
{
    public interface IMealAggregate
    {
        IIterator<IMealComponent> CreateIterator();

        int Count { get; }

        IMealComponent GetChild(int index);
    }
}
