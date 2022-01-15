using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButtonScript : MonoBehaviour
{
    public GameObject select_panel;
    public GameObject stop_botton;

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
}
