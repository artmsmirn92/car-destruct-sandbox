using mazing.common.Runtime.Ticker;
using Zenject;

public class GlobalMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISavesController>().To<SavesController>().AsSingle();
        Container.Bind<ICommonTicker>()   .To<CommonTicker>()   .AsSingle();
        Container.Bind<IUITicker>()       .To<UITicker>()       .AsSingle();
        Container.Bind<ISoundManager>()   .To<SoundManager>()   .AsSingle();
    }
}