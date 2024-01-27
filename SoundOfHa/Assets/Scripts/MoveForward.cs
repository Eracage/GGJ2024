using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed=3f;
    void Update()
    {
        transform.position += -transform.forward * Time.deltaTime * speed;
    }
}
