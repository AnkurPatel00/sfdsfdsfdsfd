using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class MasterInstaller : MonoInstaller
    {
        [SerializeField]
        private List<ConfigurationInstaller> configs = new List<ConfigurationInstaller>();

        public override void InstallBindings()
        {
            foreach (var config in configs)
            {
                if (config == null) continue;
                
                Container.Inject(config);
                config.InstallBindings();
            }
        }
    }
}