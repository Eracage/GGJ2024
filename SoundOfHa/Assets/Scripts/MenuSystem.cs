using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneNumber)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Loading Scene " + sceneNumber);
        SceneManager.LoadSceneAsync(sceneNumber);
    }

    public static void LoadSceneStatic(int sceneNumber)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Loading Scene " + sceneNumber);
        SceneManager.LoadSceneAsync(sceneNumber);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
