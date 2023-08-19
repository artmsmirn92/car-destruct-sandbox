using Lean.Localization;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;
using UnityEngine;
using Zenject;

public class GlobalMonoInstaller : MonoInstaller
{
    public GameObject leanLocalization;
    
    public override void InstallBindings()
    {
        Container.Bind<ISavesController>() .To<SavesControllerYandexGames>().AsSingle();
        Container.Bind<ICommonTicker>()    .To<CommonTicker>()    .AsSingle();
        Container.Bind<IUITicker>()        .To<UITicker>()        .AsSingle();
        Container.Bind<IViewGameTicker>()  .To<ViewGameTicker>()  .AsSingle();
        Container.Bind<ISoundManager>()    .To<SoundManager>()    .AsSingle();
        Container.Bind<IPrefabSetManager>().To<PrefabSetManager>().AsSingle();
        Container.Bind<LeanLocalization>().FromComponentInNewPrefab(leanLocalization).AsSingle();
    }
}