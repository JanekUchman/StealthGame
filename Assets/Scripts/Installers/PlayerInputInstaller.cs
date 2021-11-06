using Input;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Installers
{
    public class PlayerInputInstaller : Installer<PlayerInputInstaller>
    {
        public override void InstallBindings()
        {
            var inputActionMap = new StealthGame();
            
            Container.Bind<InputAction>()
                .WithId(BindingIDs.MovementInput)
                .FromInstance(inputActionMap.Player.Move);
            
            Container.Bind<InputAction>()
                .WithId(BindingIDs.LookInput)
                .FromInstance(inputActionMap.Player.Look);
            
            Container.Bind<InputAction>()
                .WithId(BindingIDs.CrouchInput)
                .FromInstance(inputActionMap.Player.Crouch);
            
            Container.Bind<InputAction>()
                .WithId(BindingIDs.SprintInput)
                .FromInstance(inputActionMap.Player.Sprint);
            
            Container.Bind<InputAction>()
                .WithId(BindingIDs.InteractInput)
                .FromInstance(inputActionMap.Player.Interact);
        }
    }
}
