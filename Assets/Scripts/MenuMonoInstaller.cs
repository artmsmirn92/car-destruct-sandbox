using UnityEngine;
using Zenject;

public class MenuMonoInstaller : MonoInstaller
{
    [SerializeField] private MainMenuController menuManager;
    
    public override void InstallBindings()
    {
        Container.Bind<MainMenuController>().FromInstance(menuManager).AsSingle();
    }

}
