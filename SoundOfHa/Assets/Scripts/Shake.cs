using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float shakeAngleDeg = 20;
    public float shakesPerSecond = 1f;
    Vector3 m_startRotation;
    // Start is called before the first frame update
    void Start()
    {
        m_startRotation = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float shakeZ = Mathf.PingPong(Time.time * shakesPerSecond * shakeAngleDeg * 2, shakeAngleDeg * 2) - shakeAngleDeg;
        transform.localEulerAngles = m_startRotation + new Vector3(0,0, shakeZ);
    }
}
