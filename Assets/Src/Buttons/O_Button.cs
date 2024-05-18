using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class O_Button : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;
    private Button button;
    public R_SoundEffect clickedSound;
    public CH_SoundPitch soundPlayer;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public virtual void OnClickEvent()
    {
        if (soundPlayer != null) {
            if (clickedSound != null)
            {
                soundPlayer.RaiseEvent(clickedSound);
            }
        }
    }
    public void EnableInteractable() { 
        button.interactable = true;
    }
    public void DisableInteractable()
    {
        button.interactable = false;
    }

    public void SetButtonColour(Color colour) { 
        image.color = colour;
    }
    public void SetButonTextColour(Color colour)
    {
        text.color = colour;
    }

    public void SetButonText(string _text) { 
        text.text = _text;
    }
}
