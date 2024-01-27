using Unity.VisualScripting;
using UnityEngine;

public class AggroLogic : MonoBehaviour
{
    private int m_enemyLayer;
    private int m_filterLayer;
    private GameObject m_player;
    private float m_aggroSphereRadius;

    private void Start()
    {
        m_player = transform.parent.GameObject();
        m_aggroSphereRadius = transform.GetComponent<SphereCollider>().radius;
        m_enemyLayer = LayerMask.NameToLayer("Enemy");

        var environmentMask = 1 << LayerMask.NameToLayer("Environment");
        var enemyMask = 1 << m_enemyLayer;

        m_filterLayer |= enemyMask;
        m_filterLayer |= environmentMask;
    }

    private void OnTriggerStay(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy == null)
            return;

        if (enemy.HasAggro)
            return;

        var enemyData = enemy.data;

        var distance = Vector3.Distance(m_player.transform.position, other.transform.position);
        if (distance > enemyData.engageDistance)
            return;

        var enemyDirection = other.transform.position - m_player.transform.position;
        var rayOrigin = m_player.transform.position;
        rayOrigin.y += 1;

        RaycastHit hitInfo; 
        bool rayHit = Physics.SphereCast(rayOrigin, 0.1f, enemyDirection, out hitInfo, m_aggroSphereRadius, m_filterLayer);
        if (!rayHit)
            return;

        if (hitInfo.transform.gameObject.layer != m_enemyLayer)
            return;

        enemy.Aggro();
    }
}