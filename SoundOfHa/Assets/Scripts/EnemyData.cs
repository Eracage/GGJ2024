using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public float speed = 5.0f;
    public float enragedSpeed = 7.0f;
    public float rotationSpeed = 160.0f;
    public float enragedRotationSpeed = 200.0f;
    public float health = 25.0f;
    public float damage = 10.0f;
    public float enragedDamage = 20.0f;
    public float attackRange = 2.0f;
    
    [Tooltip("Percentage of health at witch the enemy will become enraged")]
    [Range(0.0f, 1.0f)]
    public float patience = 0.5f;
    public AudioClip footstepSounds;
    public AudioClip onHitSound;
    public AudioClip onDieSound;
    
    public Sprite idleSprite;
    public Sprite enragedSprite;
}
