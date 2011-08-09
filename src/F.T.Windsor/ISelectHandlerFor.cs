using Castle.MicroKernel;

namespace F.T.Windsor
{
    public interface ISelectHandlerFor
    {
        IHandler Select(IHandler[] handlers);
    }

    public interface ISelectHandlerFor<TService> : ISelectHandlerFor
    {
    }
}
