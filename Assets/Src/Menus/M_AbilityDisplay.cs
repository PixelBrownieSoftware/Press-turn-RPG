using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class M_AbilityDisplay : M_AbilityMenu
{
    [SerializeField]
    private R_BattleCharacter selectedCharacter;
    [SerializeField]
    private B_Int[] displayButtons;
    [SerializeField]
    private List<S_Move> displayAbility = new List<S_Move>();
    [SerializeField]
    private CH_Int selectAbilityFunction;
    public TextMeshProUGUI moveDescription;

    public override void StartMenu()
    {
        page = 0;
        base.StartMenu();
        displayAbility.Clear();
        displayAbility.AddRange(selectedCharacter.battleCharacter.getCurrentDefaultMoves);
        displayAbility.AddRange(selectedCharacter.battleCharacter.getCurrentExtraMoves);
        UpdateButtons();
    }
    private void OnEnable()
    {
        selectAbilityFunction.OnFunctionEvent += GetAvailibleSkill;
    }
    private void OnDisable()
    {
        selectAbilityFunction.OnFunctionEvent -= GetAvailibleSkill;
    }

    public void UpdateButtons()
    {
        int indButton = 0;
        for (int i = 0; i < displayButtons.Length; i++)
        {
            int index = i + (displayButtons.Length * page);
            if (displayAbility == null)
            {
                displayButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (displayAbility.Count > index)
                {
                    S_Move skill = displayAbility[index];
                    Color buttonColour = Color.white;
                    SetButton(ref displayButtons[indButton], skill, index, buttonColour);
                    indButton++;
                }
                else
                {
                    displayButtons[indButton].gameObject.SetActive(false);
                    indButton++;
                }
            }
        }
    }

    public void GetAvailibleSkill(int i)
    {
        S_Move extraSkill = displayAbility[i];
        moveDescription.text = "" + DisplayAbility(extraSkill, selectedCharacter.battleCharacter);
        SetButton(ref displayButtons[i], Color.yellow);
        UpdateButtons();
        //moveDescription.text = DisplayAbility(extraSkill, bcDat);
    }
}
