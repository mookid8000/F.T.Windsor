using Castle.MicroKernel;

namespace F.T.Windsor
{
    public interface IFilterHandlersFor
    {
        IHandler[] Select(IHandler[] handlers);
    }

    public interface IFilterHandlersFor<TService>: IFilterHandlersFor
    {
    }
}