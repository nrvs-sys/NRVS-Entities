using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class ScriptableComponent : ScriptableObject, IComponent
    {
        public virtual void InitializeComponent(Entity entity) { }

        public virtual void CleanupComponent(Entity entity) { }

        public virtual void ActivateComponent(Entity entity) { }

        public virtual void DeactivateComponent(Entity entity) { }
    }
}
