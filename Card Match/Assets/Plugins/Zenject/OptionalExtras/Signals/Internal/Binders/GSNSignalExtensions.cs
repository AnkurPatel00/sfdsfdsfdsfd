using System;
namespace Zenject
{
    public static class GSNSignalExtensions
    {
        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignalWithParent<TSignal>(
            this DiContainer container)
        {
            var signalBinder = container.DeclareSignal<TSignal>();
            var signalType = typeof(TSignal);

            while (!(signalType.BaseType == typeof(object) || signalType.IsAbstract))
            {
                container.BindSignal<TSignal>()
                    .ToMethod<SignalBus>((b, s) => b.Fire(Convert.ChangeType(s, signalType.BaseType))).FromResolve();
                signalType = signalType.BaseType;
            }

            return signalBinder;
        }

        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal, TCast>(
            this DiContainer container) where TSignal : TCast
        {
            var signalBinder = container.DeclareSignal<TSignal>();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast>(s)).FromResolve();

            return signalBinder;
        }

        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal, TCast, TCast2>(
            this DiContainer container) where TSignal : TCast, TCast2
        {
            var signalBinder = container.DeclareSignal<TSignal>();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast2>(s)).FromResolve();

            return signalBinder;
        }
        
        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal, TCast, TCast2, TCast3>(
            this DiContainer container) where TSignal : TCast, TCast2, TCast3
        {
            var signalBinder = container.DeclareSignal<TSignal>();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast2>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast3>(s)).FromResolve();

            return signalBinder;
        }
        
        public static DeclareSignalRequireHandlerAsyncTickPriorityCopyBinder DeclareSignal<TSignal, TCast, TCast2, TCast3, TCast4>(
            this DiContainer container) where TSignal : TCast, TCast2, TCast3, TCast4
        {
            var signalBinder = container.DeclareSignal<TSignal>();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast2>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast3>(s)).FromResolve();
            container.BindSignal<TSignal>().ToMethod<SignalBus>((b, s) => b.Fire<TCast4>(s)).FromResolve();

            return signalBinder;
        }
    }
}