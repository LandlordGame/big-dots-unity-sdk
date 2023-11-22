using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewResetController : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;

    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1;
        scrollRect.horizontalNormalizedPosition = 0;
    }
}
