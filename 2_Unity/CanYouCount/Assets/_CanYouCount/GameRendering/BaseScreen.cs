using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScreen : MonoBehaviour, CanYouCount.IScreen
{
    [SerializeField]
    private float _animationTime = 0.5f;
    public virtual void HideScreen()
    {
        ScaleAnimation(false);
    }

    public virtual void ShowScreen()
    {
        ScaleAnimation(true);
    }

    private void ScaleAnimation(bool open) 
    {
        Vector3 scale = open ? Vector3.one : Vector3.zero;
        transform.LeanScale(scale, _animationTime)
              .setEase(LeanTweenType.easeOutBack)
              .setOnComplete(() =>
              {
                  gameObject.SetActive(open);
              });
    }
}
