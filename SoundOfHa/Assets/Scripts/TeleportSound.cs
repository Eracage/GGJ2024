using UnityEngine;

public class TeleportSound : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(gameObject);
    }
}
