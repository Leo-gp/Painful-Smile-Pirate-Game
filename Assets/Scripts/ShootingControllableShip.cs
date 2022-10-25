using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingControllableShip : ShootingShip, IControllableShip
{
    protected InputActions inputActions;

    private float moveSpeedInput;
    private float rotateSpeedInput;
    private bool slowRotateOn;

    protected override void Awake()
    {
        base.Awake();
        InitializeInputActions();
    }

    protected override void Update()
    {
        base.Update();
        moveSpeedInput = inputActions.Ship.Move.ReadValue<Vector2>().y;
        rotateSpeedInput = inputActions.Ship.Rotate.ReadValue<Vector2>().x;
    }

    public void InitializeInputActions()
    {
        inputActions = new InputActions();
        inputActions.Ship.Enable();
        inputActions.Ship.FrontalShoot.performed += FrontalShoot;
        inputActions.Ship.RightShoot.performed += RightShoot;
        inputActions.Ship.LeftShoot.performed += LeftShoot;
        inputActions.Ship.SlowRotate.performed += SetSlowRotate;
        inputActions.Ship.SlowRotate.canceled += SetSlowRotate;
    }

    private void OnEnable()
    {
        inputActions.Ship.Enable();
    }

    private void OnDisable()
    {
        inputActions.Ship.Disable();
    }

    public override void HandleMovement()
    {
        rb.AddForce(-transform.up * moveSpeedInput * BaseMoveSpeed);
    }

    public override void HandleRotation()
    {
        rb.rotation -= rotateSpeedInput * (slowRotateOn ? BaseRotateSpeed / 2f : BaseRotateSpeed);
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.GameOver();
    }

    protected override void UpdateShootCooldown()
    {
        base.UpdateShootCooldown();
        float frontalShootRemainingPercentage = remainingTimeForFrontalShoot * 100f / FrontalShootCooldown;
        float sideShootRemainingPercentage = remainingTimeForSideShoot * 100f / SideShootCooldown;
        GameManager.Instance.UpdateShootCooldownImages(frontalShootRemainingPercentage, sideShootRemainingPercentage);
    }

    private void FrontalShoot(InputAction.CallbackContext context)
    {
        FrontalShoot();
    }

    private void RightShoot(InputAction.CallbackContext context)
    {
        RightShoot();
    }

    private void LeftShoot(InputAction.CallbackContext context)
    {
        LeftShoot();
    }

    private void SetSlowRotate(InputAction.CallbackContext context)
    {
        slowRotateOn = context.performed;
    }
}