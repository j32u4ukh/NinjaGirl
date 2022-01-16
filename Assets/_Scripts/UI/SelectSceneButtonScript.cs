using UnityEngine;
using UnityEngine.UI;

public class SelectSceneButtonScript : MonoBehaviour
{
    public Sprite button_sprite;
    Image img1, img2, img3;
    int clear_level;
    BGMController bgm;

    private void Awake()
    {
        img1 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level1Button").GetComponent<Image>();
        img2 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level2Button").GetComponent<Image>();
        img3 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level3Button").GetComponent<Image>();
        bgm = GameObject.Find("BGMController").GetComponent<BGMController>();
        clear_level = PlayerPrefs.GetInt("clear_level");

        if (clear_level == 0)
        {
            img1.sprite = button_sprite;
        }
        else if (clear_level <= 1)
        {
            img1.sprite = button_sprite;
            img2.sprite = button_sprite;
        }
        else if (2 <= clear_level)
        {
            img1.sprite = button_sprite;
            img2.sprite = button_sprite;
            img3.sprite = button_sprite;
        }
    }

    public void toLevel1()
    {
        bgm.playDecisionSound();
        FadeInOut.instance.sceneFadeInOut(scene_name: "Level1");
    }

    public void toLevel2()
    {
        if(clear_level >= 1)
        {
            bgm.playDecisionSound();
            FadeInOut.instance.sceneFadeInOut(scene_name: "Level2");
        }
        else
        {
            bgm.playInvalidSound();
        }
    }

    public void toLevel3()
    {
        if (clear_level >= 2)
        {
            bgm.playDecisionSound();
            FadeInOut.instance.sceneFadeInOut(scene_name: "Level3");
        }
        else
        {
            bgm.playInvalidSound();
        }
    }

    public void toMainMenu()
    {
        bgm.playDecisionSound();
        FadeInOut.instance.sceneFadeInOut(scene_name: "MainMenu");
    }
}
