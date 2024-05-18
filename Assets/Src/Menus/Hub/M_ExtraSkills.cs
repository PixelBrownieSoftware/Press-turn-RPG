using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Unity.VisualScripting;

public class M_ExtraSkills : M_AbilityMenu
{
    [SerializeField]
    private R_Boolean isReadonly;
    [SerializeField]
    private R_MoveList extraSkills;
    [SerializeField]
    private List<S_Move> availibleSkills;
    [SerializeField]
    private List<S_Passive> availiblePassives;
    [SerializeField]
    private List<S_Move> equipSkills;
    [SerializeField]
    private R_BattleCharacter currentCharacter;
    [SerializeField]
    private B_Int[] equipButtons;
    [SerializeField]
    private B_Int[] availibleButtons;

    [SerializeField]
    private CH_Int equip;
    [SerializeField]
    private CH_Int deEquip;

    [SerializeField]
    private CH_Int equipSelect;
    [SerializeField]
    private CH_Int availibleSelect;
    [SerializeField]
    private CH_Int changePage;
    public R_Int extraSkillsMax;

    public TextMeshProUGUI moveDescription;
    public B_Int equipButton;
    public B_Int unequipButton;

    [SerializeField]
    private R_BattleCharacterList party;
    [SerializeField]
    private List<S_Move> charactersHaveMove = new List<S_Move>();
    [SerializeField]
    private S_Move selectedMove;

    public CH_SoundPitch soundPlay;
    public R_SoundEffect equipSound;
    public R_SoundEffect unEquipSound;
    public R_SoundEffect selectSound;

    public override void StartMenu()
    {
        page = 0;
        base.StartMenu();
        availibleSkills.Clear(); //currentCharacter.battleCharacter.getCurrentExtraMoves;
        availibleSkills.AddRange(extraSkills.moveListRef);
        UpdateHasMovesList();
        UpdateButtons2();
        unequipButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
    }

    public void UpdateHasMovesList() {
        charactersHaveMove.Clear();
        foreach (var chara in party.battleCharList) {
            foreach (var mv in chara.getCurrentExtraMoves) {
                if (charactersHaveMove.Contains(mv))
                {
                    continue;
                }
                charactersHaveMove.Add(mv);
            }
        }
    }
    public void ChangePage(int i)
    {
        if (i == 1) {
            if (availibleButtons.Length * (page + 1) < availibleSkills.Count) {
                page++;
                UpdateButtons2();
            }
        }
        if (i == -1) {
            if (page > 0) {
                page--;
                UpdateButtons2();
            }
        }
    }
    private void OnEnable() {
        equipSelect.OnFunctionEvent += GetEquippedSkill;
        availibleSelect.OnFunctionEvent += GetAvailibleSkill;
        changePage.OnFunctionEvent += ChangePage;
    }
    private void OnDisable()
    {
        equipSelect.OnFunctionEvent -= GetEquippedSkill;
        availibleSelect.OnFunctionEvent -= GetAvailibleSkill;
        changePage.OnFunctionEvent -= ChangePage;
    }

    private void RemoveSkill()
    {
        equipButton.gameObject.SetActive(true);
        unequipButton.gameObject.SetActive(false);
        soundPlay.RaiseEvent(unEquipSound);
        currentCharacter.battleCharacter.extraSkills.Remove(selectedMove);
        charactersHaveMove.Remove(selectedMove);
        UpdateHasMovesList();
        selectedMove = null;
        UpdateButtons2();
    }
    private void AddSkill(S_Move skill)
    {
        unequipButton.gameObject.SetActive(true);
        equipButton.gameObject.SetActive(false);
        soundPlay.RaiseEvent(equipSound);
        currentCharacter.battleCharacter.extraSkills.Add(skill);
        UpdateHasMovesList();
        selectedMove = null;
        UpdateButtons2();
    }

    public void GetAvailibleSkill(int i)
    {
        if (selectedMove != availibleSkills[i])
        {
            selectedMove = availibleSkills[i];
            moveDescription.text = "" + availibleSkills[i].name;
            unequipButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            UpdateButtons2();
            UpdateHasMovesList();
            soundPlay.RaiseEvent(selectSound);
            SetButton(ref availibleButtons[i], Color.yellow);
            moveDescription.text = DisplayAbility(selectedMove, currentCharacter.battleCharacter);
            unequipButton.SetIntButton(i);
        }
        else
        {
            if (extraSkillsMax.integer == availibleSkills.Count)
                return;
            if (currentCharacter.battleCharacter.extraSkills.Contains(selectedMove))
            {
                RemoveSkill();
            }
            else
            {
                AddSkill(availibleSkills[i]);
            }
        }
    }
    public void GetEquippedSkill(int i)
    {

        if (selectedMove != currentCharacter.battleCharacter.extraSkills[i])
        {
            selectedMove = currentCharacter.battleCharacter.extraSkills[i];
            moveDescription.text = "" + selectedMove.name;
            unequipButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            UpdateButtons2();
            UpdateHasMovesList();
            soundPlay.RaiseEvent(selectSound);
            SetButton(ref equipButtons[i], Color.yellow);
            moveDescription.text = DisplayAbility(selectedMove, currentCharacter.battleCharacter);
        }
        else
        {
            if (extraSkillsMax.integer == availibleSkills.Count)
                return;
            if (currentCharacter.battleCharacter.extraSkills.Contains(selectedMove))
            {
                RemoveSkill();
            }
        }
    }

    public void UpdateButtons2() {
        int indButton = 0;
        for (int i = 0; i < availibleButtons.Length; i++) {
            int index = i + (availibleButtons.Length * page);
            if (availibleSkills == null)
            {
                availibleButtons[i].gameObject.SetActive(false);
            }
            else
            {
                if (availibleSkills.Count > index)
                {
                    availibleButtons[indButton].EnableInteractable();
                    S_Move skill = availibleSkills[index];
                    Color buttonColour = Color.white;
                    if (extraSkillsMax.integer == currentCharacter.battleCharacter.extraSkills.Count)
                        buttonColour = Color.grey;
                    else if (extraSkillsMax.integer > availibleSkills.Count)
                    {
                        if (skill.MeetsRequirements(currentCharacter.battleCharacter))
                            buttonColour = Color.white;
                        else
                            buttonColour = Color.red;
                    }
                    if (charactersHaveMove.Contains(skill))
                    {
                        if (currentCharacter.battleCharacter.extraSkills.Contains(skill))
                        {
                            buttonColour = Color.green;
                        }
                        else
                        {
                            buttonColour = Color.blue;
                            availibleButtons[indButton].DisableInteractable();
                        }
                    }
                    else {
                        if (currentCharacter.battleCharacter.getCurrentDefaultMoves.Contains(skill))
                        {
                            buttonColour = Color.magenta;
                            availibleButtons[indButton].DisableInteractable();
                        }
                    }

                    SetButton(ref availibleButtons[indButton], skill, index, buttonColour);
                    indButton++;
                }
                else
                {
                    availibleButtons[indButton].gameObject.SetActive(false);
                    indButton++;
                }
            }
        }
        indButton = 0;
        for (int i = 0; i < equipButtons.Length; i++)
        {
            if (currentCharacter.battleCharacter.extraSkills.Count > i)
            {
                S_Move skill = currentCharacter.battleCharacter.extraSkills[i];
                /*
                equipButtons[indButton].gameObject.SetActive(true);
                equipButtons[indButton].SetIntButton(i);
                equipButtons[indButton].SetButonText(skill.name);
                */
                SetButton(ref equipButtons[indButton], skill, i, Color.white);
                indButton++;
            }
            else
            {
                equipButtons[indButton].gameObject.SetActive(false);
                indButton++;
            }
        }
    }
}
