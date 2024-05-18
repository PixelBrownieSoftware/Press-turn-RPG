using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class s_hpBoxGUI : MonoBehaviour
{
    public Text comboText;
    public Image comboImage;
    public GameObject comboObj;
    public Image HPboxColour;
    public Image StatusEff;

    public Text hpText;
    public Text spText;
    Text hpTextShadow;
    Text spTextShadow;

    public O_BattleCharacter bc;

    public Slider hpSlider;
    public Slider spSlider;

    public Image hpBR;
    public Image spBR;
    public Image HPPic;
    public Image Arrow;
    public Material mat;
    public MaterialPropertyBlock mt;
    public Renderer rend;

    public GameObject ob;

    public R_BattleCharacter currentCharacter;

    public Sprite poisionIcon;
    public Sprite stunIcon;
    public Sprite confuseIcon;
    public Sprite defaultImage;

    float statusTimer = 0f;
    const float statusFlipTimer = 0.5f;
    int statusFlipIndex = 0;
    int prevStatusFlipCount = 0;

    private void Awake()
    {
        hpTextShadow = hpText.transform.GetChild(0).GetComponent<Text>();
        spTextShadow = spText.transform.GetChild(0).GetComponent<Text>();
        HideComboImage();
    }

    public void SetMaterial() {
        if (bc == null)
        {
            ob.SetActive(false);
        } else
        {
            ob.SetActive(true);
            HPPic.color = bc.baseCharacterData.characterColour;
            /*if (bc.battleCharData.battleImage != null)
            {
                HPPic.sprite = bc.battleCharData.battleImage;
            }
            else
            {
                HPPic.sprite = defaultImage;
            }*/
            HPPic.sprite = defaultImage;
        }
    }


    public void Update()
    {
        if (bc != null)
        {
            if (bc == currentCharacter.battleCharacter)
            {
                Arrow.gameObject.SetActive(true);
            }
            else
            {
                Arrow.gameObject.SetActive(false);
            }
            ob.SetActive(true);
            hpSlider.value = ((float)bc.characterHealth.health / (float)bc.characterHealth.maxHealth) * 100;
            spSlider.value = ((float)bc.characterHealth.stamina / (float)bc.characterHealth.maxStamina) * 100;
            if (bc.baseCharacterData != null)
            {
                hpBR.color = bc.baseCharacterData.characterColour;
                spBR.color = bc.baseCharacterData.characterColour2;
            }

            hpText.text = "" + bc.characterHealth.health;
            hpTextShadow.text = "" + bc.characterHealth.health;

            #region STATUS EFFECTS
            List<string> statusEffs = new List<string>();
            foreach (S_StatusInstance stat in bc.statusEffects) {
                /*
                switch (stat.status) {
                    case STATUS_EFFECT.POISON:
                        statusEffs.Add("psn");
                        break;
                    case STATUS_EFFECT.STUN:
                        statusEffs.Add("stn");
                        break;

                    case STATUS_EFFECT.CONFUSED:
                        statusEffs.Add("con");
                        break;
                }
                */
            }
            if (statusEffs.Count > 0)
            {
                if (prevStatusFlipCount != statusEffs.Count)
                {
                    statusFlipIndex = 0;
                }
                if (statusTimer > 0)
                {
                    statusTimer -= Time.deltaTime;
                }
                else
                {
                    statusTimer = statusFlipTimer;
                    if (statusFlipIndex < statusEffs.Count - 1)
                        statusFlipIndex++;
                    else
                        statusFlipIndex = 0;
                }
                switch (statusEffs[statusFlipIndex])
                {
                    case "psn":
                        StatusEff.sprite = poisionIcon;
                        break;
                    case "stn":
                        StatusEff.sprite = stunIcon;
                        break;
                    case "con":
                        StatusEff.sprite = confuseIcon;
                        break;
                }
                StatusEff.color = Color.white;
            }
            else {
                StatusEff.color = Color.clear;
            }
            
            #endregion


        }
        else {

            Arrow.gameObject.SetActive(false);
            ob.SetActive(false);
        }
    }

    public void ChangeComboImage(S_Move mov) {
        if (mov != null)
        {
            comboObj.SetActive(true);
            comboText.text = mov.name;
        }
    }
    public void HideComboImage()
    {
        comboObj.SetActive(false);
    }
}
