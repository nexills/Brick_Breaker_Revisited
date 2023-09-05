using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject SelectionPanel;
    public GameObject classic;
    public GameObject endless;
    public GameObject random;
    public TMP_Text diff_text;
    public Slider difficulty;
    // Start is called before the first frame update
    void Start() {
        Info.Gamemode = 0;
        SelectionPanel.SetActive(false);
        classic.SetActive(false);
        endless.SetActive(false);
        random.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        Info.diff = (int) difficulty.value;
        switch(Info.diff) {
            case 0:
                diff_text.text = "Difficulty: Casual";
                break;
            case 1:
                diff_text.text = "Difficulty: Relaxed";
                break;
            case 2:
                diff_text.text = "Difficulty: Standard";
                break;
            case 3:
                diff_text.text = "Difficulty: Advanced";
                break;
            case 4:
                diff_text.text = "Difficulty: Hard";
                break;
        }
    }

    public void play() {
        // called when play button is pressed

        if (SelectionPanel.active == false) {
            SelectionPanel.SetActive(true);
            classic.SetActive(true);
            endless.SetActive(true);
            random.SetActive(true);

        } else {
            SelectionPanel.SetActive(false);
            classic.SetActive(false);
            endless.SetActive(false);
            random.SetActive(false);
        }
        
    }

    public void playClassic() {
        // called when classic play mode is chosen
        Info.Gamemode = 1;
        SceneManager.LoadScene("Play");
    }

    public void playEndless() {
        // called when endless is chosen
        SceneManager.LoadScene("Play");
        Info.Gamemode = 2;
    }

    public void playRandom() {
        // called if random is chosen
        SceneManager.LoadScene("Play");
        Info.Gamemode = 3;
    }

    public void getHelp() {
        SceneManager.LoadScene("Help");
    }

}
