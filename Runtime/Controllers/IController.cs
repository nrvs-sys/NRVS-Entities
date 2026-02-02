using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// The "Brain" of an Entity. Could be a Player, or an AI!
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Called when a Controller is added to an Entity.
        /// </summary>
        void InitializeController(Entity entity);

        /// <summary>
        /// Called when a Controller is removed from an Entity.
        /// </summary>
        void CleanupController(Entity entity);

        /// <summary>
        /// Called when a Controller's Entity is activated (or when the component is added to an active Entity).
        /// </summary>
        void ActivateController(Entity entity);

        /// <summary>
        /// Called when a Controller's Entity is deactivated.
        /// </summary>
        void DeactivateController(Entity entity);
    }
}
