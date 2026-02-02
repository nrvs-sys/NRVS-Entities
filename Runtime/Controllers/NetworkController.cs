using FishNet.Object;

namespace Core
{
    public abstract class NetworkController : NetworkBehaviour, IController
    {
        protected Entity entity;

        public virtual void InitializeController(Entity entity)
        {
            this.entity = entity;
        }

        public virtual void CleanupController(Entity entity)
        {
            this.entity = null;
        }

        public virtual void ActivateController(Entity entity) { }

        public virtual void DeactivateController(Entity entity) { }
    }
}
