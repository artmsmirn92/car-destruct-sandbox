using UnityEngine;
using Zenject;

public class Level01MonoInstaller : MonoInstaller
{
    [SerializeField] private TubeWithBalls tubeWithBalls;
    
    public override void InstallBindings()
    {
        Container.Bind<TubeWithBalls>().FromInstance(tubeWithBalls).AsSingle();
    }
}