using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using UnityEngine.UI;

public class PopDown : MonoBehaviour
{
    public ShellController shell;
    public Sprite unknown;
    public int index;

    public void MenuOpen()
    {
        foreach (PopDown p in GetComponentsInChildren<PopDown>())
        {
            p.LockCheck();
        }
    }
    public void LockCheck()
    {
        if(unknown != null)
        {
            string truth = PlayerPrefs.GetString(index.ToString(), "");
            if (truth != "true")
            {
                GetComponent<Image>().sprite = unknown;
            }
            else
            {
                shell.DisplayStar(GetComponent<Image>(), index);
                LeanTween.scale(this.gameObject, new Vector3(17f,17f,1), 1f).setEaseInOutBack().setDelay(1f);
                LeanTween.scale(this.gameObject, new Vector3(15f,15f,1), 1f).setEaseInOutBack().setDelay(2f);
            }
        }
    }

    public void OnClick()
    {
        string truth = PlayerPrefs.GetString(index.ToString(), "");
        if (truth != "true")
        {
            GetComponent<Image>().sprite = unknown;
            shell.PopUp(12);
        }
        else
        {
            shell.PopUp(index);
        }
        AudioManager.instance.Play("Bloop");
    }
    public void Suicide()
    {
        LeanTween.scale(this.gameObject, new Vector3(0,0,0), 0.25f).setEaseInCubic().setOnComplete(KYS);
    }

    void KYS()
    {
        Destroy(this.gameObject);
    }

    public void Enlarge()
    {
        LeanTween.scale(this.gameObject, new Vector3(50f,50f,1), 0.5f);
        LeanTween.move(this.gameObject, new Vector3(0,0,5), 0.5f);
    }
}
