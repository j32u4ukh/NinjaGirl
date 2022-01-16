using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject select_panel;
    public GameObject stop_botton;
    public GameObject level_select_botton;
    public GameObject main_menu_botton;
    public GameObject replay_botton;


    public void setSelectPanelOn()
    {
        select_panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void setSelectPanelOff()
    {
        select_panel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void setStopBottonOn()
    {
        stop_botton.SetActive(true);
    }

    public void setStopBottonOff()
    {
        stop_botton.SetActive(false);
    }

    public void clickPlayButton()
    {
        GameObject player = GameObject.Find("Player");
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("Run", true);

        GameObject play_btn = GameObject.Find("Canvas/SaveAreaPanel/PlayButton");
        play_btn.SetActive(false);

        StartCoroutine(waitForPlayerRunningOut());
    }

    IEnumerator waitForPlayerRunningOut()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("LevelSelect");
    }

    public void resetButton()
    {
        RectTransform rect = GameObject.Find("Canvas/SaveAreaPanel/ResetImage").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, -100f);
    }

    public void yesButton()
    {
        noButton();
        PlayerPrefs.DeleteAll();
    }

    public void noButton()
    {
        RectTransform rect = GameObject.Find("Canvas/SaveAreaPanel/ResetImage").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, 1500f);
    }

    public void levelSelectButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelect");
    }

    public void mainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void replayButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
