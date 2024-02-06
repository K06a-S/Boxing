using Boxing.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Boxing
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private Canvas _worldCanvas;

        public override void InstallBindings()
        {
            Container.Bind<GameInput>().FromInstance(new GameInput()).AsSingle().NonLazy();
            Container.Bind<Canvas>().FromInstance(_worldCanvas).AsSingle().NonLazy();
        }
    }
}