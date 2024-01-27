using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSlimiter : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
    }

}
