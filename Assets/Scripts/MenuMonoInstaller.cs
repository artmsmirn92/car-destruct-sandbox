using UnityEngine;
using Zenject;

public class MenuMonoInstaller : MonoInstaller
{
    [SerializeField] private MenuManager menuManager;
    
    public override void InstallBindings()
    {
        Container.Bind<MenuManager>().FromInstance(menuManager).AsSingle();
    }

}
