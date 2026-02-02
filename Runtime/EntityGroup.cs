using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Group_ New", menuName = "Entities/Entity Group")]
public class EntityGroup : ScriptableObject
{
    public string groupName;

    public List<EntityType> entityTypes;
}
