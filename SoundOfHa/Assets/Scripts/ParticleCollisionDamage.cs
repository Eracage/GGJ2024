using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionDamage : MonoBehaviour
{
    public ParticleSystem partSystem;
    public List<ParticleCollisionEvent> collisionEvents;
    public float damagePerParticle = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = partSystem.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (!rb)
        {
            Debug.Log("Collided to" + other.gameObject);
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();

        if (!enemy)
        {
            return;
        }

        int i = 0;
        while (i < numCollisionEvents)
        {
            enemy.TakeDamage(damagePerParticle);
            i++;
        }
    }
}
