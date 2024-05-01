using Game.CommonModules.Pooling.Audio;
using Game.CommonModules.Pooling;

namespace Game.Installers
{
    public class GameInstaller : ConfigurationInstaller
    {
        public PoolConfigVO PoolConfigVo;

        public override void InstallBindings()
        {
            Container.Bind<IAudioPlayer>().To<AudioPlayer>().AsSingle().NonLazy();

            Container.Bind<PoolConfigVO>().FromInstance(PoolConfigVo).AsSingle();
            Container.Bind<IPoolManager>().To<PoolManager>().FromComponentInHierarchy().AsCached();
        }
    }
}