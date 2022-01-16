using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneButtonScript : MonoBehaviour
{
    public Sprite button_sprite;
    Image img1, img2, img3;
    int clear_level;

    private void Awake()
    {
        img1 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level1Button").GetComponent<Image>();
        img2 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level2Button").GetComponent<Image>();
        img3 = GameObject.Find("Canvas/SaveAreaPanel/SelectPanelBGImage/Level3Button").GetComponent<Image>();

        clear_level = PlayerPrefs.GetInt("clear_level", 0);

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
        SceneManager.LoadScene("Level1");
    }

    public void toLevel2()
    {
        if(clear_level >= 1)
        {
            SceneManager.LoadScene("Level2");
        }
    }

    public void toLevel3()
    {
        if (clear_level >= 2)
        {
            SceneManager.LoadScene("Level3");
        }
    }

    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
