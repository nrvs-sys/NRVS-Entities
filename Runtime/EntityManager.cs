using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using System;
using GameKit.Dependencies.Utilities;

namespace Core
{
    [CreateAssetMenu(fileName = "Entity Manager_ New", menuName = "Entities/Entity Manager")]
    public class EntityManager : ManagedObject
    {
        [SerializeField]
        List<EntityGroup> entityGroups = new();

        [Space(10)]

        [SerializeField]
        EntityGroup playerEntityGroup;
        [SerializeField]
        EntityGroup enemyEntityGroup;
        [SerializeField]
        EntityGroup pickupEntityGroup;

        [NonSerialized]
        private List<Entity> entityList = new();

        private Dictionary<EntityType, List<Entity>> entityTypeDictionary = new();

        private Dictionary<EntityGroup, List<Entity>> entityGroupDictionary = new();

        public Entity[] entities => entityList.ToArray();
        public Entity[] nonPlayerEntities => GetEntitiesNotInGroup(playerEntityGroup);        
        public Entity[] playerEntities => GetEntitiesInGroup(playerEntityGroup);
        public Entity[] enemyEntities => GetEntitiesInGroup(enemyEntityGroup);
        public Entity[] pickupEntities => GetEntitiesInGroup(pickupEntityGroup);

        public int entityCount => entityList.Count;

        public delegate void EntityRegisteredHandler(Entity entity);
        public event EntityRegisteredHandler OnEntityRegistered;
        public event EntityRegisteredHandler OnEntityUnregistered;

        protected override void Initialize() { }

        protected override void Cleanup()
        {
            Reset();
        }

        public void Reset()
        {
            entityList.Clear();
            entityTypeDictionary.Clear();
            entityGroupDictionary.Clear();
        }

        public Entity[] GetEntitiesOfType(EntityType entityType) => entityTypeDictionary.TryGetValue(entityType, out var entityList) ? entityList.ToArray() : new Entity[0];

        public Entity[] GetEntitiesInGroup(EntityGroup group) => entityGroupDictionary.TryGetValue(group, out var entityList) ? entityList.ToArray() : new Entity[0];

        public Entity[] GetEntitiesNotInGroup(EntityGroup group)
        {
            List<Entity> cachedEntitiesInGroup = CollectionCaches<Entity>.RetrieveList();
            
            GetEntitiesNotInGroupNonAlloc(group, ref cachedEntitiesInGroup);

            var outputArray = cachedEntitiesInGroup.ToArray();

            CollectionCaches<Entity>.Store(cachedEntitiesInGroup);

            return outputArray;
        }

        public List<Entity> GetEntitiesOfTypeNonAlloc(EntityType entityType, ref List<Entity> outputList)
        {
            outputList.Clear();

            if (entityTypeDictionary.TryGetValue(entityType, out var entityList))

                outputList.AddRange(entityList);

            return outputList;
        }

        public List<Entity> GetEntitiesInGroupNonAlloc(EntityGroup group, ref List<Entity> outputList)
        {
            outputList.Clear();

            if (entityGroupDictionary.TryGetValue(group, out var entityList))
                outputList.AddRange(entityList);

            return outputList;
        }

        public List<Entity> GetEntitiesNotInGroupNonAlloc(EntityGroup group, ref List<Entity> outputList)
        {
            outputList.Clear();

            List<Entity> cachedEntitiesInGroup = CollectionCaches<Entity>.RetrieveList();

            GetEntitiesInGroupNonAlloc(group, ref cachedEntitiesInGroup);

            foreach (var entity in entities)
            {
                if (!cachedEntitiesInGroup.Contains(entity))
                    outputList.Add(entity);
            }

            CollectionCaches<Entity>.Store(cachedEntitiesInGroup);

            return outputList;
        }

        public int GetEntityTypeCount(EntityType entityType) => entityTypeDictionary.TryGetValue(entityType, out var entityList) ? entityList.Count : 0;

        public int GetEntityGroupCount(EntityGroup entityGroup) => entityGroupDictionary.TryGetValue(entityGroup, out var entityList) ? entityList.Count : 0;

        public void RegisterEntity(Entity entity)
        {
            if (entity.entityType != null)
            {

                if (!entityTypeDictionary.TryGetValue(entity.entityType, out var el))
                {
                    el = new();
                    entityTypeDictionary.Add(entity.entityType, el);
                }

                if (!el.Contains(entity))
                    el.Add(entity);


                foreach (var entityGroup in entityGroups)
                {
                    if (entityGroup.entityTypes.Contains(entity.entityType))
                    {
                        if (entityGroupDictionary.TryGetValue(entityGroup, out el))
                        {
                            if (!el.Contains(entity))
                                el.Add(entity);
                        }
                        else
                        {
                            el = new();
                            el.Add(entity);
                            entityGroupDictionary.Add(entityGroup, el);
                        }
                    }
                }
            }

            if (!entityList.Contains(entity))
            {
                entityList.Add(entity);
                OnEntityRegistered?.Invoke(entity);

               // Debug.Log($"Entity Registered: {entity.entityType} - {entity.name} ({entityList.Count})", entity.gameObject);
            }
        }

        public void UnregisterEntity(Entity entity)
        {
            if (entity.entityType != null)
            {
                if (entityTypeDictionary.TryGetValue(entity.entityType, out var el))
                    el.Remove(entity);


                foreach (var entityGroupList in entityGroupDictionary)
                {
                    if (entityGroupList.Key.entityTypes.Contains(entity.entityType))
                        entityGroupList.Value.Remove(entity);
                }
            }

            if (entityList.Remove(entity))
            {
                OnEntityUnregistered?.Invoke(entity);

                //Debug.Log($"Entity Unregistered: {entity.entityType} - {entity.name} ({entityList.Count})", entity.gameObject);
            }
        }

        public void DespawnEntities()
        {
            foreach (var entity in entities)
                entity.Despawn();
        }

        public void DespawnEntities(EntityType entityType)
        {
            foreach (var entity in GetEntitiesOfType(entityType))
                entity.Despawn();
        }

        public void DespawnEntities(EntityGroup entityGroup)
        {
            foreach (var entity in GetEntitiesInGroup(entityGroup))
                entity.Despawn();
        }
    }
}
