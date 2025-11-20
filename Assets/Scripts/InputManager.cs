using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DentedPixel;

public enum Dir
{
    Right, Left, Up, Down
}

public class InputManager : MonoBehaviour
{
    public GM gm;
    public SpectralManager spectralManager;
    public GameObject Circle;
    public AudioManager audio;

    private void Update()
    {
        if (gm.state == GameState.Playing)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                gm.Shift(Dir.Right);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                gm.Shift(Dir.Left);
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                gm.Shift(Dir.Up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                gm.Shift(Dir.Down);
            }

        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void Enable(CanvasGroup panel)
    {
        foreach(CanvasGroup group in GetComponentsInChildren<CanvasGroup>())
        {
            group.alpha = 0;
            group.blocksRaycasts = false;
        }
        LeanTween.alphaCanvas(panel, 1f, 0.5f);
        panel.blocksRaycasts = true;
        audio.Play("Bloop");
    }

    public void Disable(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.blocksRaycasts = false;
        LeanTween.alphaCanvas(GetComponentsInChildren<CanvasGroup>()[0], 1f, 0.5f);
        GetComponentsInChildren<CanvasGroup>()[0].blocksRaycasts = true;
    }
    public void ExplodeTransition(CanvasGroup panel)
    {
        audio.Play("Supernova");
        GameObject c = Instantiate(Circle);
        LeanTween.scale(c, new Vector3(1,1,1), 0.5f);
        LeanTween.scale(c, new Vector3(3,3,1), 0.2f).setDelay(0.5f);
        LeanTween.alpha(c, 0, 0.3f).setDelay(0.7f).setDestroyOnComplete(true);
        Enable(panel);
    }
    
}
