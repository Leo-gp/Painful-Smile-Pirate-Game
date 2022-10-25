using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [field: SerializeField] public Vector2 Offset { get; private set; }
    [field: SerializeField] public Slider Slider { get; private set; }

    public Ship Target { get; set; }

    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        transform.position = Target.transform.position + (Vector3)Offset;
    }

    public void DecreaseHealth(Ship ship, float value)
    {
        Slider.value -= value / ship.MaxHealth;
    }
}