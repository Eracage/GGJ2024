using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f;
    public float mouseSensitivity = 400.0f;

    public float damage = 10.0f;

    public GameObject m_DecalPrefab;
    public SphereCollider m_interactCollider;

    private Vector3 velocity;
    private CharacterController controller;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private Transform cameraTransform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.TransformDirection(new Vector3(moveHorizontal, 0, moveVertical)) * speed;

        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(0.0f, rotationX, 0.0f);
        cameraTransform.localRotation = Quaternion.Euler(-rotationY, 0.0f, 0.0f);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;  // Small downward force when on the ground to keep the player grounded
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(movement * Time.deltaTime + velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (!m_interactCollider)
        {
            return;
        }

        var collisions = Physics.OverlapSphere(m_interactCollider.transform.position, m_interactCollider.radius);
        foreach (var c in collisions)
        {
            Debug.Log("Collided with", c);
            var target = c.GetComponent<Interactable>();
            if (target)
            {
                Debug.Log("Interacting");
                target.Interact();
            }
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            else
            {
                GameObject decal = Instantiate(m_DecalPrefab, hit.point, cameraTransform.rotation);
                Destroy(decal, 10.0f);
            }
        }
    }
}