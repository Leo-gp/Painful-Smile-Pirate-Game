using UnityEngine;

[System.Serializable]
public struct ShipDeteriorationLevel
{
    [field: SerializeField] public float Health { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}