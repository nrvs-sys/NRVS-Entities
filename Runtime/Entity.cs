using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Core;

public sealed class Entity : NetworkBehaviour
{
    [Header("Events")]
    public UnityEvent<Entity> onSpawn;
    public UnityEvent<Entity> onDespawn;


    public enum SearchStyle
    {
        None,
        Recursive,
        NonRecursive
    }

    public EntityType entityType;

    public EntityManager entityManager;

    [Space(10)]
    [SerializeField]
    private SearchStyle controllerSearchStyle = SearchStyle.Recursive;

    [Space(10)]
    [SerializeField]
    private SearchStyle componentSearchStyle = SearchStyle.Recursive;
    [SerializeField]
    private List<ScriptableComponent> scriptableComponents = new();

    /// <summary>
    /// Determines whether the Entity's Controllers & Components are active. This is separate from GameObject's "Active" state.
    ///
    /// Set this flag with the `SetActive()` method.
    /// </summary>
    public bool isEntityActive { get; private set; } = false;

    public bool isEntityInitialized { get; private set; } = false;

    private IController _controller;
    public IController controller
    {
        get => _controller;

        set
        {
            if (_controller != value)
            {
                if (_controller != null)
                {
                    if (isEntityActive)
                        _controller.DeactivateController(this);

                    _controller.CleanupController(this);
                }

                var oldController = _controller;
                _controller = value;


                if (_controller != null)
                {
                    _controller?.InitializeController(this);

                    if (isEntityActive)
                        _controller.ActivateController(this);
                }

                OnControllerChanged?.Invoke(oldController, _controller);
            }
        }
    }

    public delegate void ControllerChangedHandler(IController oldController, IController newController);
    public event ControllerChangedHandler OnControllerChanged;

    public delegate void ActiveHandler(Entity entity, bool isActive);
    public event ActiveHandler OnEntityActiveChanged;

    public List<IComponent> components { get; private set; } = new();

    /// <summary>
    /// The GameObject that spawned this entity
    /// </summary>
    public GameObject spawner { get; set; }

    public Vector3 spawnPosition { get; private set; }
    public Quaternion spawnRotation { get; private set; }

    public uint spawnTick { get; private set; }
    public float spawnTime { get; private set; }

    private void OnDestroy()
    {
        if (isEntityInitialized)
        {
            Debug.LogWarning($"Entity {entityType} was destroyed without being despawned! Cleaning up..");

            Cleanup();
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        spawnTick = TimeManager.Tick;
        spawnTime = (float)TimeManager.TicksToTime(spawnTick);

        InitializeEntity();
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();

        if (isEntityInitialized)
            Cleanup();
    }

    public void InitializeEntity(bool activate = true)
    {
        if (isEntityInitialized)
        {
            Debug.LogWarning($"Entity {entityType}: skipping Initialize as Entity is already initialized!");
            return;
        }

        // Search for ScriptableComponents
        foreach (var component in scriptableComponents)
            if (component != null && !components.Contains(component)) components.Add(component);

        // Search for MonoComponents and NetworkComponents on the Prefab
        switch (componentSearchStyle)
        {
            case SearchStyle.Recursive:
                foreach (var component in gameObject.GetComponentsInChildren<IComponent>())
                    if (component != null) components.Add(component);
                break;
            case SearchStyle.NonRecursive:
                foreach (var component in gameObject.GetComponents<IComponent>())
                    if (component != null) components.Add(component);
                break;
        }

        // Initialize All Components
        foreach (var component in components)
            component.InitializeComponent(this);

        // Search for MonoControllers and NetworkControllers on the Prefab
        switch (controllerSearchStyle)
        {
            case SearchStyle.Recursive:
                controller = gameObject.GetComponentInChildren<IController>();
                break;
            case SearchStyle.NonRecursive:
                controller = gameObject.GetComponent<IController>();
                break;
        }

        isEntityInitialized = true;

        if (activate)
            SetActive(true);

        entityManager?.RegisterEntity(this);

        onSpawn?.Invoke(this);
    }

    public void Cleanup()
    {
        if (!isEntityInitialized)
        {
            Debug.LogWarning($"Entity {entityType}: skipping Cleanup as Entity is not initialized!");
            return;
        }

        onDespawn?.Invoke(this);

        entityManager?.UnregisterEntity(this);

        SetActive(false);

        foreach (var component in components)
            component?.CleanupComponent(this);

        controller = null;

        components.Clear();

        isEntityInitialized = false;
    }

    public void SetActive(bool active)
    {
        if (isEntityActive != active)
        {
            isEntityActive = active;

            //Debug.Log($"Entity {gameObject.name} is now {(isEntityActive ? "active" : "inactive")}.");

            if (isEntityActive)
            {
                controller?.ActivateController(this);

                foreach (var component in components)
                    component.ActivateComponent(this);
            }
            else
            {
                controller?.DeactivateController(this);

                foreach (var component in components)
                    component.DeactivateComponent(this);
            }

            OnEntityActiveChanged?.Invoke(this, isEntityActive);
        }
    }

    public T GetEntityComponent<T>() where T : IComponent => (T)components.Find(c => c is T);

    public void DespawnEntity() => DespawnEntity(false);

    public void DespawnEntity(bool allowClientToDespawn)
    {
        if (!IsSpawned)
        {
            Debug.LogWarning($"Entity {entityType} is not spawned. Cannot despawn.");
            return;
        }

        if (IsServerInitialized)
            Despawn();
        else
        {
            if (allowClientToDespawn)
                Rpc_Despawn();
            //else
            //    Debug.LogWarning($"Entity {entityType} - Cannot despawn from client. To enable this, set the `allowClientToDespawn` arg when calling `DespawnEntity()`");
        }

    }

    [ServerRpc]
    private void Rpc_Despawn() => Despawn();

    public override string ToString() => $"Entity: {entityType}";
}