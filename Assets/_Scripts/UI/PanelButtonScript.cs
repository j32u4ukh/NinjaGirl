using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject select_panel;
    public GameObject stop_botton;
    public GameObject level_select_botton;
    public GameObject main_menu_botton;
    public GameObject replay_botton;
    BGMController bgm;

    private void Awake()
    {
        bgm = GameObject.Find("BGMController").GetComponent<BGMController>();
    }

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
        bgm.playDecisionSound();
        stop_botton.SetActive(true);
    }

    public void setStopBottonOff()
    {
        bgm.playDecisionSound();
        stop_botton.SetActive(false);
    }

    public void clickPlayButton()
    {
        GameObject player = GameObject.Find("Player");
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("Run", true);
        bgm.playDecisionSound();

        GameObject play_btn = GameObject.Find("Canvas/SaveAreaPanel/PlayButton");
        play_btn.SetActive(false);
        FadeInOut.instance.sceneFadeInOut("LevelSelect");
    }

    public void resetButton()
    {
        RectTransform rect = GameObject.Find("Canvas/SaveAreaPanel/ResetImage").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, -100f);
        bgm.playDecisionSound();
    }

    public void yesButton()
    {
        noButton();
        PlayerPrefs.DeleteAll();
        IsFirstPlayCheck ifpc = GameObject.Find("IsFirstPlayCheck").GetComponent<IsFirstPlayCheck>();
        ifpc.firstTimePlayState();
    }

    public void noButton()
    {
        RectTransform rect = GameObject.Find("Canvas/SaveAreaPanel/ResetImage").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0f, 1500f);
        bgm.playDecisionSound();
    }

    public void levelSelectButton()
    {
        bgm.playDecisionSound();
        FadeInOut.instance.sceneFadeInOut("LevelSelect");
    }

    public void mainMenuButton()
    {
        bgm.playDecisionSound();
        FadeInOut.instance.sceneFadeInOut("MainMenu");
    }

    public void replayButton()
    {
        bgm.playDecisionSound();
        FadeInOut.instance.sceneFadeInOut(scene_name: SceneManager.GetActiveScene().name);
    }
}
