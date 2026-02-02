using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace Core
{
    public abstract class NetworkComponent : NetworkBehaviour, IComponent
    {
        public virtual void InitializeComponent(Entity entity) { }

        public virtual void CleanupComponent(Entity entity) { }

        public virtual void ActivateComponent(Entity entity) { }

        public virtual void DeactivateComponent(Entity entity) { }
    }
}
