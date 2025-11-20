using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using DentedPixel;

public class SpectralManager : MonoBehaviour
{
    public ShellController shell;
    public GM gm;
    public Image card;
    public Text title;
    public Button play;
    public GameObject youSure;
    public void Pause()
    {
        play.interactable = true;
        title.text = "Paused";
        shell.DisplayStar(card, shell.currentNum);
    }

    public void GameOver()
    {
        play.interactable = false;
        title.text = "Game Over";
        GetComponentInParent<InputManager>().ExplodeTransition(GetComponent<CanvasGroup>());
        switch (shell.currentNum)
        {
            case 0:
                shell.DisplayStar(card, 13);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                shell.DisplayStar(card, 9);
                break;
            case 6:
            case 7:
            case 8:
                shell.DisplayStar(card, 10);
                break;
            default:
                shell.DisplayStar(card, 11);
                break;
        }     
    }
}
