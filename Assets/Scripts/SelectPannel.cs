using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPannel : MonoBehaviour, IPointerExitHandler
{
    public GameObject selectPannel;
    public void OnPointerExit(PointerEventData eventData)
    {
        selectPannel.SetActive(false);
    }
}
