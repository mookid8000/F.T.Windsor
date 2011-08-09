What?
====

`FtwFacility` is a facility for the excellent [Castle Windsor][3] that addresses one of its few shortcomings: The nifty `IHandlerSelector` and `IHandlersFilter` hooks are not registered in the container, they are instances provided to the container at registration time, and thus cannot take advantage of autowiring, lifestyle management, etc.

Therefore, `FtwFacility` goes on and registers a `FtwHandlerSelector` that has access to the kernel, allowing it to pull handler selectors and handler filters from the container.

With `FtwFacility`, you can register `ISelectHandlerFor<TService>` and `IFilterHandlersFor<TService>` implementations and have them resolved and called whenever `TService` is either `Resolve`d or `ResolveAll`d.

Why?
====

Like I said: Your usual handler selector/filter is not pulled from the container. With this facility, you can hook into the same places _with components from the container_.

How?
====

Reference F.T.Windsor from your project and

    var container = new WindsorContainer();

    container.AddFacility<FtwFacility>();

before adding any selectors to the container.

For example, you can do this:

    container.Register(Component.For<ISelectHandlerFor<ISomeService>>().ImplementedBy<SomeServiceSelector>(),
                       Component.For<ISomeService>().ImplementedBy<FirstImpl>(),
                       Component.For<ISomeService>().ImplementedBy<SecondImpl>());

    var someService = container.Resolve<ISomeService>();

in order to choose among handlers for `FirstImpl` and `SecondImpl` when resolving `ISomeService`, and this:

    container.Register(Component.For<IFilterHandlersFor<ISomeService>>().ImplementedBy<SomeServiceFilter>(),
                       Component.For<ISomeService>().ImplementedBy<FirstImpl>(),
                       Component.For<ISomeService>().ImplementedBy<SecondImpl>());

    var services = container.ResolveAll<ISomeService>();

in order to pick & choose & order the list of handlers invoked when resolving multimple implementations of `ISomeService`.

More info
====

Contact me [through Twitter][2].

License
====

RespectOrderDirectivesHandlersFilter is [Beer-ware][1].

[1]: http://en.wikipedia.org/wiki/Beerware
[2]: http://twitter.com/#!/mookid8000
[3]: http://docs.castleproject.org/Windsor.MainPage.ashx