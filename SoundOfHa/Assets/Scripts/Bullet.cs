using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50.0f;
    public float damage = 10.0f;
    public float lifeTime = 3.0f;

    public GameObject m_DecalPrefab;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision other) 
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        else
        {
            GameObject decal = Instantiate(m_DecalPrefab, other.contacts[0].point, transform.rotation);
            Destroy(decal, 10.0f);
        }
        Destroy(gameObject);
    }

}
