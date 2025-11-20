using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShellController : MonoBehaviour
{
    public Image Star;
    public ParticleSystem stars;
    public ParticleSystem.EmissionModule emission;
    public Text lastElement;
    public Text tutorial;
    public int currentNum;
    private List<Image> shells = new List<Image>();
    public List<Sprite> cards;
    public GameObject popPanel;
    public AudioManager audio = AudioManager.instance;

    private void Start() 
    {
        emission = stars.emission;
    }

    public void AddShell(int num, Color color, string text)
    {
        if(currentNum < num)
        {
            this.GetComponentInParent<Canvas>().overrideSorting = true;
            Image shell = Instantiate(Star);
            shells.Add(shell);
            shell.color = color;
            shell.transform.SetParent(Star.transform.parent);
            shell.transform.position = Star.transform.position;
            shell.transform.localScale = new Vector3(0,0,0);
            lastElement.GetComponent<Text>().text = text;
            if(num == 8)
                lastElement.color = Color.white;
            lastElement.transform.SetAsLastSibling();
            LeanTween.scale(shell.gameObject, new Vector3(1.25f,1.25f,1.25f), 0.25f).setEaseInOutQuad();
            LeanTween.scale(shell.gameObject, new Vector3(1 - (float)num/10,1 - (float)num/10, 1), 0.25f).setEaseSpring().setDelay(0.25f);
            currentNum = num;
            emission.rateOverTime = 10+currentNum*2;
            audio.Play("Bloop");
            LeanTween.alphaText(tutorial.rectTransform, 0, 2f);
            if (num < 8 && PlayerPrefs.GetString(currentNum.ToString(), "") != "true")
            {
                PopUp(currentNum);
            }

        }
    }

    public void ResetShells()
    {
        foreach(Image shell in shells)
        {
            Destroy(shell);
        }
        currentNum = 0;
        lastElement.text = "H";
    }

    public void DisplayStar(Image card, int index)
    {
        switch (index)
        {
            case 0:
                card.sprite = cards[0];
                break;
            case 1:
                card.sprite = cards[0];
                PlayerPrefs.SetString(1.ToString(),"true");
                break;
            case 2:
                card.sprite = cards[1];
                PlayerPrefs.SetString(2.ToString(), "true");
                break;
            case 3:
                card.sprite = cards[2];
                PlayerPrefs.SetString(3.ToString(), "true");
                break;
            case 4:
                card.sprite = cards[3];
                PlayerPrefs.SetString(4.ToString(), "true");
                break;
            case 5:
                card.sprite = cards[4];
                PlayerPrefs.SetString(5.ToString(), "true");
                break;
            case 6:
                card.sprite = cards[5];
                PlayerPrefs.SetString(6.ToString(), "true");
                break;
            case 7:
                card.sprite = cards[6];
                PlayerPrefs.SetString(7.ToString(), "true");
                break;
            case 8:
                card.sprite = cards[6];
                break;
            case 9:
                card.sprite = cards[7];
                PlayerPrefs.SetString(9.ToString(), "true");
                break;
            case 10:
                card.sprite = cards[8];
                PlayerPrefs.SetString(10.ToString(), "true");
                break;
            case 11:
                card.sprite = cards[9];
                PlayerPrefs.SetString(11.ToString(), "true");
                break;
            case 13:
                card.sprite = cards[11];
                break;
            default:
                card.sprite = cards[10];
                break;
        }
    }
    public void PopUp(int index)
    {
        GameObject pop = Instantiate(popPanel);
        DisplayStar(pop.GetComponentInChildren<Button>().image, index);
        pop.transform.SetParent(GetComponentInParent<Canvas>().transform);
        pop.transform.SetAsLastSibling();
        LeanTween.scale(pop, new Vector3(1.2f,1.2f,1.2f), 0.5f).setEaseOutBack();
    }

    public void GameObjectPop(GameObject panel)
    {
        GameObject pop = Instantiate(panel);
        pop.transform.SetParent(GetComponentInParent<Canvas>().transform);
        pop.transform.SetAsLastSibling();
        pop.transform.localScale = new Vector3(0,0,1f);
        LeanTween.scale(pop, new Vector3(1f,1f,500f), 0.5f);
    }
}
