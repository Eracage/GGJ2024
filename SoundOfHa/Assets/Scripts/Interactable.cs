using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{

    public UnityEvent interactEvent;
    public AudioClip[] audioClips;

    Collider m_collider;
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider>();
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    protected void playAudio()
    {
        if (audioClips.Length > 0)
        {
            int clip = Random.Range(0, audioClips.Length);
            if (m_audioSource)
            {
                m_audioSource.PlayOneShot(audioClips[clip]);
            }
            else
            {
                AudioSource.PlayClipAtPoint(audioClips[clip], transform.position);
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting with", this);
        interactEvent.Invoke();
        interact();
    }

    virtual protected void interact()
    {
        playAudio();
    }
}
