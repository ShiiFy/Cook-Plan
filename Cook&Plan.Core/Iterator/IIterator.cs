
namespace Cook_Plan.Core.Iterator
{
    public interface IIterator<T>
    {
        void First();
        void Next();
        bool IsDone();
        T CurrentItem();
    }
}
