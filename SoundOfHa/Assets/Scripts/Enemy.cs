using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyData data;
    public bool HasAggro { private set; get; } = false;

    public float m_CurrentHealth { get; set; }
    private bool isEnraged = false;
    private Transform target;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    [SerializeField] SpriteRenderer[] m_bodySprites;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = transform.gameObject.GetComponentInChildren<Animator>();
        m_Agent.speed = data.speed;
        m_Agent.angularSpeed = data.rotationSpeed;
        m_CurrentHealth = data.health;
        GetComponentInChildren<AudioSource>().clip = data.footstepSounds;
        GetComponentInChildren<AudioSource>().Play();

        if(!(data.enragedSprites.Length == data.idleSprites.Length && data.enragedSprites.Length == m_bodySprites.Length))
        {
            Debug.LogError("Sprites and data are not the same length!");
        }
    }

    void Update()
    {
        if (HasAggro)
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

            if(data.onDiePrefab != null)
            {
                Instantiate(data.onDiePrefab, transform.position, Quaternion.identity);
            }

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

            for(int i = 0; i< data.idleSprites.Length; i++)
            {
                m_bodySprites[i].sprite = data.enragedSprites[i];
            }
        }
    }

    public void Aggro()
    {
        HasAggro = true;
        m_Animator.SetTrigger("StartWalking");
    }
}
