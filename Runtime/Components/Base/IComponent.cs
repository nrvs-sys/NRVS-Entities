namespace Core
{
    /// <summary>
    /// The basic building block of an Entity.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// Called when a Component is added to an Entity.
        /// </summary>
        void InitializeComponent(Entity entity);

        /// <summary>
        /// Called when a Component is removed from an Entity.
        /// </summary>
        void CleanupComponent(Entity entity);

        /// <summary>
        /// Called when a Component's Entity is activated (or when the component is added to an active Entity).
        /// </summary>
        void ActivateComponent(Entity entity);

        /// <summary>
        /// Called when a Component's Entity is deactivated.
        /// </summary>
        void DeactivateComponent(Entity entity);
    } 
}
