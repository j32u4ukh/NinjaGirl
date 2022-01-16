using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut instance;
    public GameObject fade_obj;
    public Animator animator;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void sceneFadeInOut(string scene_name)
    {
        StartCoroutine(fadeInOut(scene_name));
    }

    IEnumerator fadeInOut(string scene_name)
    {
        fade_obj.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene(scene_name);
        animator.Play("FadeOut");

        // FadeOut 動畫長度為 1 秒
        yield return new WaitForSecondsRealtime(1.0f);
        fade_obj.SetActive(false);

        Time.timeScale = 1.0f;
    }
}
