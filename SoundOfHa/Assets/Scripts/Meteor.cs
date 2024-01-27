using UnityEngine;

public class Meteor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            MenuSystem.LoadSceneStatic(1);
        }
    }
}
