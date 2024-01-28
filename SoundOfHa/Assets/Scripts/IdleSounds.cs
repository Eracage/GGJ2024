using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSounds : MonoBehaviour
{
    public float minTime = 5.0f;
    public float maxTime = 15.0f;

    float m_time;
    public AudioClip[] clips;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        m_time = Random.Range(0, maxTime - minTime);
        StartCoroutine(repeatingPlayClip());
    }

    IEnumerator repeatingPlayClip()
    {
        yield return new WaitForSeconds(m_time);
        randTime();
        playAudio();
        yield return repeatingPlayClip();
    }

    void playAudio()
    {
        if (clips.Length < 1)
            return;

        int clip = Random.Range(0, clips.Length);
        
        if (audioSource)
        {
            audioSource.PlayOneShot(clips[clip]);
        }
        else
        {
            AudioSource.PlayClipAtPoint(clips[clip], transform.position);
        }
    }

    void randTime()
    {
        m_time = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
