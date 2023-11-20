using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public FadeInOut fade;

    public IEnumerator ChangeScene()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")  
        {
            StartCoroutine(ChangeScene());
        }
    }
}
