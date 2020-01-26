using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour, CanYouCount.IScreen
{
    public virtual void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public virtual void ShowScreen()
    {
        gameObject.SetActive(true); 
    }
}
