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
        if (m_audioSource && audioClips.Length > 0)
        {
            int clip = Random.Range(0, audioClips.Length);
            m_audioSource.PlayOneShot(audioClips[clip]);
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
