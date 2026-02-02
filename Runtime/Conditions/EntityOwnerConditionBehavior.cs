using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition_ Entity_ Owner_ New", menuName = "Behaviors/Conditions/Entity/Owner")]
public class EntityOwnerConditionBehavior : ConditionBehavior<Entity>
{
    protected override bool Evaluate(Entity value) => value.TryGetComponent<NetworkObject>(out var networkObject) && networkObject.IsOwner;
}
