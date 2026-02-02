using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

[CreateAssetMenu(fileName = "Condition_ Entity Type_ New", menuName = "Behaviors/Conditions/Entity Type")]
public class EntityTypeConditionBehavior : ConditionBehavior<Entity>
{
    [SerializeField]
    EntityType entityType;

    protected override bool Evaluate(Entity entity) => entity.entityType == entityType;
}