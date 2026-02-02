using UnityEngine;

namespace Core
{
    public abstract class ScriptableController : ScriptableObject, IController
    {
        public virtual void InitializeController(Entity entity) { }

        public virtual void CleanupController(Entity entity) { }

        public virtual void ActivateController(Entity entity) { }

        public virtual void DeactivateController(Entity entity) { }
    }
}
