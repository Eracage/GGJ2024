using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MagicalSuperWizardOfFun : MonoBehaviour
{
    NavMeshAgent m_agent;

    public Vector3[] m_waypoints;

    private bool m_hasInteracted = false;

    public float m_flySpeed;

    public GameObject m_particles;


    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.destination = m_waypoints[0];
    }

    void Update()
    {
        if(m_hasInteracted)
        {
            transform.position += Vector3.up * m_flySpeed * Time.deltaTime;
            return;
        }

        if(Vector3.Distance(transform.position, m_agent.destination) < 0.3f)
        {
            m_agent.destination = m_waypoints[Random.Range(0, m_waypoints.Length)];
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach(Vector3 waypoint in m_waypoints)
        {
            Gizmos.DrawSphere(waypoint, 0.5f);
        }
    }

    public void InteractWithWizard()
    {
        m_agent.enabled = false;
        m_hasInteracted = true;
        m_particles.SetActive(true);
        Destroy(gameObject, 25.0f);
    }

}
