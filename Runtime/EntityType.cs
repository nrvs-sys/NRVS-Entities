using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using System;

[CreateAssetMenu(fileName = "Entity Type_ New", menuName = "Entities/Entity Type")]
public class EntityType : ScriptableObject
{
    public string entityName;

    public override string ToString() => entityName;
}