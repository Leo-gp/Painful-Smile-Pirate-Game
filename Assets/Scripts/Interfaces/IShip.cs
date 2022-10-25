using System.Collections.Generic;
using UnityEngine;

public interface IShip
{
    float BaseMoveSpeed { get; }
    float BaseRotateSpeed { get; }
    float MaxHealth { get; }
    HealthBar HealthBarPrefab { get; }
    GameObject FirePrefab { get; }
    float FireTime { get; }
    List<ShipDeteriorationLevel> ShipDeteriorationLevels { get; }

    void HandleMovement();
    void HandleRotation();
    void TakeDamage(float damage);
    void Die();
    void InstantiateHealthBar();
}