using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(IdleSounds))]
public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyData data;
    public bool HasAggro { private set; get; } = false;

    public float m_CurrentHealth { get; set; }
    private bool isEnraged = false;
    private Transform target;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;
    private AudioSource audioSource;
    [SerializeField] SpriteRenderer[] m_bodySprites;
    [SerializeField] ParticleSystem m_particleSystem;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = transform.gameObject.GetComponentInChildren<Animator>();
        m_Agent.speed = data.speed;
        m_Agent.angularSpeed = data.rotationSpeed;
        m_CurrentHealth = data.health;
        audioSource = GetComponentInChildren<AudioSource>();
        audioSource.clip = data.footstepSounds;
        GetComponent<IdleSounds>().clips = data.idleSounds;

        if(!(data.enragedSprites.Length == data.idleSprites.Length && data.enragedSprites.Length == m_bodySprites.Length))
        {
            Debug.LogError("Sprites and data are not the same length!");
        }
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) < data.attackRange)
        {
            MenuSystem.LoadSceneStatic(5);
        }
        

        if (HasAggro)
            m_Agent.destination = target.position;
    }

    public void TakeDamage(float damage)
    {
        m_particleSystem.Emit(1);
        Aggro();
        m_CurrentHealth -= damage;
        if (m_CurrentHealth <= 0)
        {
            playClip(data.onDieSound);

            if (data.onDiePrefab != null)
            {
                Instantiate(data.onDiePrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
            return;
        }

        playClip(data.onHitSound);

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

    void playClip(AudioClip sound)
    {
        if (!sound)
            return;
        GameObject hitSound = Instantiate(new GameObject("HitSound"), transform.position, Quaternion.identity);
        hitSound.AddComponent<AudioSource>().clip = sound;
        hitSound.GetComponent<AudioSource>().Play();
        Destroy(hitSound, sound.length + 0.1f);
    }

    public void Aggro()
    {
        if (HasAggro)
            return;
        HasAggro = true;
        audioSource.Play();

        playClip(data.aggroSound);

        m_Animator.SetTrigger("StartWalking");
    }
}
