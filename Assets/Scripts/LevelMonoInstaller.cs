using UnityEngine;
using Zenject;

public class LevelMonoInstaller : MonoInstaller
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private LevelMenuController levelMenuController;
    
    public override void InstallBindings()
    {
        Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle();
        Container.Bind<LevelMenuController>().FromInstance(levelMenuController).AsSingle();
    }
}