using UnityEngine;

public class Meteor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("PlayerDieSound").GetComponent<TeleportSound>().Play();
            MenuSystem.LoadSceneStatic(5);
        }
    }
}
