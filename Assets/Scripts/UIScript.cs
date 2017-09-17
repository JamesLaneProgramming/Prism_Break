using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour {

    public GameObject start_Button;
    public GameObject exit_Button;
    public GameObject background_Panel;
    public GameObject credits_Panel;


	public void play()
    {
        background_Panel.SetActive(false);
        start_Button.SetActive(false);
        exit_Button.SetActive(false);
    }

    public void show_Credits()
    {
        background_Panel.SetActive(true);
        credits_Panel.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
}
