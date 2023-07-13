using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EventManager eventManager;

    public override void InstallBindings()
    {
        Container.Bind<PlayerManager>().FromInstance(playerManager).AsSingle();
        Container.Bind<EventManager>().FromInstance(eventManager).AsSingle();
    }
}