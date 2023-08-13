using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    private GameObject main;
    private GameObject play;
    private GameObject options;
    private GameObject credits;
    
    // Start is called before the first frame update
    void Start()
    {
        main = transform.GetChild(0).gameObject;
        main.SetActive(true);

        play = transform.GetChild(1).gameObject;
        play.SetActive(false);

        options = transform.GetChild(2).gameObject;
        options.SetActive(false);

        credits = transform.GetChild(3).gameObject;
        credits.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void main_play() 
    {
        main.SetActive(false);
        play.SetActive(true);
    }

    public void main_options() 
    {
        main.SetActive(false);
        options.SetActive(true);

    }

    public void main_credits() 
    {
        main.SetActive(false);
        credits.SetActive(true);

    }

    public void main_exit() 
    {
        main.SetActive(false);
        Application.Quit();
    }

    public void options_back() {
        options.SetActive(false);
        main.SetActive(true);
    }

    public void play_back() {
        play.SetActive(false);
        main.SetActive(true);
    }

    public void play_newgame() {
        play.SetActive(false);
        SaveData.spawnPoint = new Vector3(0, 2, 0);
        SceneManager.LoadScene("MainWorld");

    }

    public void play_continue() {
        play.SetActive(false);
        SceneManager.LoadScene("MainWorld");
    }

    public void credits_back() {
        credits.SetActive(false);
        main.SetActive(true);
    }

}
