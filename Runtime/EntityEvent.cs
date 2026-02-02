using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityEvent
{
    public Entity entity { get; }
    public Vector3 position { get; }
    public float time { get; }
}

[Serializable]
public struct EntityEvent : IEntityEvent
{
    [Serializable]
    public struct EventData
    {
        public string key;
        public string value;

        public EventData(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public EventData(string key, int value) : this(key, value.ToString()) { }
        public EventData(string key, float value) : this(key, value.ToString()) { }
        public EventData(string key, bool value) : this(key, value.ToString()) { }
    }

    public Entity entity { get; }
    public Vector3 position { get; }
    public float time { get; }
    public EventData[] data { get; }

    public EntityEvent(Entity entity, Vector3 position, float time, EventData[] data = null)
    {
        this.entity = entity;
        this.position = position;
        this.time = time;
        this.data = data;
    }

    #region Data Getters

    public bool TryGetInt(string key, out int value)
    {
        value = 0;
        if (data == null)
            return false;
        foreach (var item in data)
        {
            if (item.key == key)
            {
                if (int.TryParse(item.value, out value))
                    return true;
            }
        }
        return false;
    }

    public bool TryGetFloat(string key, out float value)
    {
        value = 0;
        if (data == null)
            return false;
        foreach (var item in data)
        {
            if (item.key == key)
            {
                if (float.TryParse(item.value, out value))
                    return true;
            }
        }
        return false;
    }

    public bool TryGetString(string key, out string value)
    {
        value = null;
        if (data == null)
            return false;
        foreach (var item in data)
        {
            if (item.key == key)
            {
                value = item.value;
                return true;
            }
        }
        return false;
    }

    public bool TryGetBool(string key, out bool value)
    {
        value = false;
        if (data == null)
            return false;
        foreach (var item in data)
        {
            if (item.key == key)
            {
                if (bool.TryParse(item.value, out value))
                    return true;
            }
        }
        return false;
    }

    #endregion
}

[Serializable]
public struct EntityDamageEvent : IEntityEvent
{
    public Entity entity { get; }
    public EntityType damagingEntity { get; }
    public string damageSource { get; }
    public Vector3 position { get; }
    public float time { get; }
    public float damageAmount { get; }
    public EntityDamageEvent(Entity entity, EntityType damagingEntity, string damageSource, Vector3 position, float time, float damageAmount)
    {
        this.entity = entity;
        this.damagingEntity = damagingEntity;
        this.damageSource = damageSource;
        this.position = position;
        this.time = time;
        this.damageAmount = damageAmount;
    }
}

[Serializable]
public struct EntityDeathEvent : IEntityEvent
{
    public Entity entity { get; }
    public EntityType killingEntity { get; }
    public string damageSource { get; }
    public Vector3 position { get; }
    public float time { get; }

    public EntityDeathEvent(Entity entity, EntityType killingEntity, string damageSource, Vector3 position, float time)
    {
        this.entity = entity;
        this.killingEntity = killingEntity;
        this.damageSource = damageSource;
        this.position = position;
        this.time = time;
    }
}

public struct GemCollectedEvent : IEntityEvent
{
    public Entity entity { get; }
    public Entity collectingEntity { get; }
    public Vector3 position { get; }
    public float time { get; }
    public float score { get; }

    public GemCollectedEvent(Entity entity, Entity collectingEntity, Vector3 position, float time, float score)
    {
        this.entity = entity;
        this.collectingEntity = collectingEntity;
        this.position = position;
        this.time = time;
        this.score = score;
    }
}
