using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 target;
    float timeElapsed;
    public float lerpDuration = 0.8f;

    void Update()
    {
        if (target == null)
            return;


        float distance = Vector3.Distance(target, transform.position);
        if (distance > 0.01f)
        {
            if (timeElapsed < lerpDuration)
            {
                transform.position = Vector3.Lerp(transform.position, target, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
