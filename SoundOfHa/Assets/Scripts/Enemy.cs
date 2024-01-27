using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyData data;

    public float m_CurrentHealth { get; set; }
    private bool isEnraged = false;
    private Transform target;
    private NavMeshAgent m_Agent;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = data.speed;
        m_Agent.angularSpeed = data.rotationSpeed;
        m_CurrentHealth = data.health;
        GetComponentInChildren<SpriteRenderer>().sprite = data.idleSprite;
        GetComponentInChildren<AudioSource>().clip = data.footstepSounds;
        GetComponentInChildren<AudioSource>().Play();
    }

    void Update()
    {
        m_Agent.destination = target.position;
    }

    public void TakeDamage(float damage)
    {
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            GameObject deathSound = Instantiate(new GameObject("DeathSound"), transform.position, Quaternion.identity);
            deathSound.AddComponent<AudioSource>().clip = data.onDieSound;
            deathSound.GetComponent<AudioSource>().Play();
            Destroy(deathSound, data.onDieSound.length + 0.1f);
            Destroy(gameObject);
            return;
        }

        GameObject hitSound = Instantiate(new GameObject("HitSound"), transform.position, Quaternion.identity);
        hitSound.AddComponent<AudioSource>().clip = data.onHitSound;
        hitSound.GetComponent<AudioSource>().Play();
        Destroy(hitSound, data.onHitSound.length + 0.1f);

        if (m_CurrentHealth/data.health <= data.patience && !isEnraged)
        {
            isEnraged = true;
            m_Agent.speed = data.enragedSpeed;
            m_Agent.angularSpeed = data.enragedRotationSpeed;
            GetComponentInChildren<SpriteRenderer>().sprite = data.enragedSprite;
        }
    }
}
