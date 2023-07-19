using Zenject;

public class GlobalMonoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISavesController>().To<SavesController>().AsSingle();
    }
}