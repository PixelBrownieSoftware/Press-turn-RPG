using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_GuiList : MonoBehaviour
{
    Image[] items;
    public int amount;
    public TextMeshProUGUI text;

    void Awake()
    {
        items = GetComponentsInChildren<Image>();
    }

    public void Update()
    {
        if (items != null)
        {
            amount = Mathf.Clamp(amount, 0, items.Length);
            if(text != null)
                text.text = "" + amount;
            for (int i = 0; i < items.Length; i++)
            {
                if (i > (amount - 1))
                {
                    items[i].gameObject.SetActive(false);
                }
                else
                {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
