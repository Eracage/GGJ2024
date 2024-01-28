using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Image DashEffectImage;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 400.0f;
    public float dashDistance = 7.0f;
    public float dashCooldown = 2.0f;

    public float damage = 10.0f;

    public AudioClip[] shootingSounds;
    public AudioSource shootingSource;

    public GameObject m_DecalPrefab;
    public SphereCollider m_interactCollider;

    private Vector3 velocity;
    private CharacterController controller;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    public float firerate = 0.3f;
    private float nextFire = 0.0f;
    private float nextDash = 0.0f;

    public float staminaMax = 10.0f;
    public float stamina = 5.0f;
    public float staminaRegen = 2.0f;
    public float staminaDashCost = 6.0f;

    private Transform cameraTransform;

    public GameObject projectilePrefab;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        stamina = Mathf.Clamp(stamina + staminaRegen * Time.deltaTime, 0, staminaMax);
        

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

        velocity.y += gravity * Time.deltaTime;

        controller.Move(movement * Time.deltaTime + velocity * Time.deltaTime);

        if (Input.GetButton("Fire1"))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + firerate;
                Shoot();
            }   
        }

        if (Input.GetButtonDown("Fire2"))
        {
            stamina -= 4.0f;
            Interact();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDash)
        {
            Dash();
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
        shootingSource.clip = shootingSounds[Random.Range(0, shootingSounds.Length)];
        shootingSource.Play();

        RaycastHit hit;
        Vector3 hitTarget = cameraTransform.position + cameraTransform.forward * 50.0f;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
        {
            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(DamageAfterDelay(damageable, 0.3f));
            }
            else
            {
                StartCoroutine(DecalAfterDelay(hit.point, cameraTransform.rotation, 0.3f));
            }
            hitTarget = hit.point;
        }
        GameObject bullet = Instantiate(projectilePrefab, transform.position, cameraTransform.rotation);
        bullet.GetComponent<Bullet>().target = hitTarget;
    }

    IEnumerator DamageAfterDelay(IDamageable damageable, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(damageable.m_CurrentHealth>0)
            damageable.TakeDamage(damage);
    }

    IEnumerator DecalAfterDelay(Vector3 position,Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject decal = Instantiate(m_DecalPrefab, position, rotation);
        Destroy(decal, 10.0f);
    }

    
    private void Dash()
    {
        Vector3 dashDirection = controller.velocity.normalized;
        StartCoroutine(SmoothDash(dashDirection, dashDistance, 0.3f));
        StartCoroutine(DashEffect());
        nextDash = Time.time + dashCooldown;
    }

    private IEnumerator SmoothDash(Vector3 direction, float distance, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * distance;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private IEnumerator DashEffect()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        float strength;
        while (elapsedTime < (duration/2))
        {
            float t = elapsedTime / duration;
            strength = Mathf.Lerp(0,1,t);
            elapsedTime += Time.deltaTime;
            DashEffectImage.material.SetFloat("_Effect", strength);
            yield return null;
        }
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            strength = Mathf.Lerp(1, 0, t);
            elapsedTime += Time.deltaTime;
            DashEffectImage.material.SetFloat("_Effect", strength);
            yield return null;
        }
    }

}