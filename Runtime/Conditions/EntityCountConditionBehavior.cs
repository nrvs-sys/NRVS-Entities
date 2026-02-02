using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition_ Entity Count_ New", menuName = "Behaviors/Conditions/Entity Count")]
public class EntityCountConditionBehavior : ConditionBehavior
{
    [SerializeField]
    EntityManager entityManager;

    [SerializeField]
    EntityType entityType;

    [SerializeField]
    MathComparisonType comparisonType;

    [SerializeField]
    int count;

    protected override bool Evaluate() => MathComparison.Compare(comparisonType, entityManager.GetEntityTypeCount(entityType), count);
}
