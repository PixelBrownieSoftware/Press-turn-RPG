using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class s_elementalWeaknessGUI : MonoBehaviour
{
    public Image weakImg;
    public Image affinityIcon;
    public Sprite unknown;
    public Sprite weak;
    public Sprite bad;
    public Sprite normal;
    O_BattleCharacter bcD;
    public S_Element el;

    public Color voidDMG;
    public Color resist;
    public Color absorb;
    public Color reflect;
    float elementWeakness;
    TURN_FLAG aff;
    public R_BattleCharacter currentCharacter;
    public CH_Func setData;
    //public S_EnemyWeaknessReveal revealAffinity;
    bool showAffinity = true;

    private void OnEnable()
    {
        setData.OnFunctionEvent += SetToDat;
    }

    private void OnDisable()
    {
        setData.OnFunctionEvent -= SetToDat;
    }

    public void SetToDat() {
        showAffinity = true;
        bcD = currentCharacter.battleCharacter;
        elementWeakness = bcD.GetElementWeakness(el);
        /*
        if (revealAffinity != null) {
            showAffinity = revealAffinity.EnemyWeaknessExists(bcD.characterDataSource, el);
        }
        */
        if (showAffinity)
        {
            if (elementWeakness >= 2)
                aff = TURN_FLAG.WEAK;
            else if (elementWeakness < 2 && elementWeakness > 0)
                aff = TURN_FLAG.NORMAL;
            else if (elementWeakness == 0)
                aff = TURN_FLAG.NULL;
            else if (elementWeakness < 0 && elementWeakness > -1)
                aff = TURN_FLAG.REPEL;
            else if (elementWeakness <= -1)
                aff = TURN_FLAG.ABSORB;
        }
    }

    void Update()
    {
        if (bcD != null)
        {
            if (showAffinity)
            {
                switch (aff)
                {
                    case TURN_FLAG.WEAK:
                        affinityIcon.sprite = weak;
                        break;

                    case TURN_FLAG.NORMAL:
                        affinityIcon.sprite = normal;
                        break;

                    default:
                        affinityIcon.sprite = bad;
                        break;
                }
            }
            else
            {
                affinityIcon.sprite = unknown;
            }
        }
    }
}
