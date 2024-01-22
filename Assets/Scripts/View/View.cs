using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    //ÏÔÊ¾
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    //Òþ²Ø
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
