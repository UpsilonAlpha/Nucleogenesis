using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popper : MonoBehaviour
{
    public void Pop()
    {
        this.transform.SetAsLastSibling();
        this.transform.localScale = new Vector3(0,0,1f);
        LeanTween.scale(this.gameObject, new Vector3(1f,1f,500f), 0.5f);
    }
    public void Suicide()
    {
        LeanTween.scale(this.gameObject, new Vector3(0,0,0), 0.25f).setEaseInCubic();
    }
}
