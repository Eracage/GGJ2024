using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonEvent : MonoBehaviour
{
    public KeyCode key;
    public UnityEvent keyDown;
    public UnityEvent keyUp;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            keyDown.Invoke();
        }
        if (Input.GetKeyUp(key))
        {
            keyUp.Invoke();
        }
    }
}
