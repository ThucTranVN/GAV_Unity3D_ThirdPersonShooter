using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public Animator rigController;
    public float jumpHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    public float groundSpeed;
    public float pushPower;

    private CharacterController characterController;
    private Animator playerAnimator;
    private ActiveWeapon activeWeapon;
    private WeaponReload weaponReload;
    private Vector2 playerInput;
    private Vector3 rootMotion;
    private Vector3 velocity;
    private bool isJumping;
    private int isSprintingParam = Animator.StringToHash("isSprinting");
    
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        weaponReload = GetComponent<WeaponReload>();
    }

    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        playerAnimator.SetFloat("InputX", playerInput.x);
        playerAnimator.SetFloat("InputY", playerInput.y);

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

    private bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = weaponReload.isReloading;
        return isSprinting && !isFiring && !isReloading;
    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        playerAnimator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
    }

    private void UpdateOnGround()
    {
        Vector3 stepForward = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        characterController.Move(stepForward + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!characterController.isGrounded)
        {
            SetInAir(0);
        }  
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        characterController.Move(displacement);
        isJumping = !characterController.isGrounded;
        rootMotion = Vector3.zero;
        playerAnimator.SetBool("isJumping", isJumping);
    }

    private void OnAnimatorMove()
    {
        rootMotion += playerAnimator.deltaPosition;
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = playerAnimator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        playerAnimator.SetBool("isJumping", true);
    }

    private Vector3 CalculateAirControl()
    {
        return ((transform.forward * playerInput.y) + (transform.right * playerInput.x)) * (airControl / 100);
    }
}
