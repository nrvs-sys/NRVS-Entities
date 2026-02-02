using System;

namespace Core
{
    [Serializable]
    public class SerializedController : IController
    {
        public virtual void InitializeController(Entity entity) { }

        public virtual void CleanupController(Entity entity) { }

        public virtual void ActivateController(Entity entity) { }

        public virtual void DeactivateController(Entity entity) { }
    }
}
