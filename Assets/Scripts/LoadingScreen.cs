using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public float loadingTime = 1f;
    private void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }
    
    private IEnumerator LoadAsyncOperation()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync("Menu");
            
        while (!loading.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(loadingTime);
    }
}
