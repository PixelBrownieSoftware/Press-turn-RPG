using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class M_AbilityMenu : S_MenuSystem
{
    protected int page = 0;

    public void SetButton(ref B_SkillSelect button, Color colour)
    {
        button.gameObject.SetActive(true);
        button.SetButtonColour(colour);
    }
    public void SetButton(ref B_SkillSelect button, S_Move skill, int index, Color colour)
    {
        button.gameObject.SetActive(true);
        button.SetIntButton(index);
        button.cost.amount = skill.cost;
        button.SetButonText(skill.name);
        button.SetButtonColour(colour);
    }
    public void SetButton(ref B_SkillSelect button, S_Move skill, int index)
    {
        SetButton(ref button, skill, index, Color.white);
    }
    public string DisplayAbility(S_Move ability, O_BattleCharacter bcDat)
    {
        S_Move extraSkill = ability;
        S_Stats skillStatRequire = extraSkill.statReq;

        bool strReqFufil = bcDat.strength >= skillStatRequire.strength;
        bool vitReqFufil = bcDat.vitality >= skillStatRequire.vitality;
        bool dexReqFufil = bcDat.dexterity >= skillStatRequire.dexterity;
        bool agiReqFufil = bcDat.agility >= skillStatRequire.agility;
        bool lucReqFufil = bcDat.luck >= skillStatRequire.luck;
        bool intReqFufil = bcDat.magicPow >= skillStatRequire.magicPow;

        bool canEquip = strReqFufil && vitReqFufil && dexReqFufil && agiReqFufil && lucReqFufil && intReqFufil;

        string strengthReq = "";
        string vitalityReq = "";
        string dexterityReq = "";
        string agilityReq = "";
        string luckReq = "";
        string magpowReq = "";

        string scope = "";
        string inflict = "";
        string isHealDesc = "";
        string accuracy = "Accuracy: " + (ability.accuracy * 100f) + "%";
        string power = "Power: " + ability.power;
        if (ability.isPassive)
        {
            isHealDesc = "";
        } else
        {
            if (ability.isHeal)
            {
                isHealDesc = "healing";
            }
            else
            {
                isHealDesc = "damage";
            }
            switch (ability.targetScope)
            {
                case S_Move.TARGET_SCOPE.AOE:
                    scope += "Area of effect " + isHealDesc + " on";
                    break;
                case S_Move.TARGET_SCOPE.SINGLE:
                    scope += "Single target " + isHealDesc + " on";
                    break;
                case S_Move.TARGET_SCOPE.ALL:
                    scope += "All " + isHealDesc + " on";
                    break;
            }
            switch (ability.factionScope)
            {
                case S_Move.FACTION_SCOPE.FOES:
                    scope += " enemies.";
                    break;
                case S_Move.FACTION_SCOPE.ALLY:
                    scope += " allies.";
                    break;
                case S_Move.FACTION_SCOPE.ALL:
                    scope += " all.";
                    break;
            }
        }
        if (ability.statusInflict != null) {
            if (ability.statusInflict.Length > 0) {
                foreach (var status in ability.statusInflict) {
                    string statusDesc = "";
                    if (status.chance < 1f)
                    {
                        statusDesc += (status.chance * 100f) + "%" + " chance of ";
                    }
                    statusDesc += status.add_remove ? "inflicting " : "removing ";
                    statusDesc += status.statusEffect.name;
                    inflict += statusDesc + "\n";
                }
            }
        }

        if (strReqFufil)
        {
            if (skillStatRequire.strength != 0)
                strengthReq = "<color=green>" + skillStatRequire.strength + "</color>";
        }
        else
            strengthReq = "<color=red>" + skillStatRequire.strength + "</color>";
        if (vitReqFufil)
        {
            if (skillStatRequire.vitality != 0)
                vitalityReq = "<color=green>" + skillStatRequire.vitality + "</color>";
        }
        else
            vitalityReq = "<color=red>" + skillStatRequire.vitality + "</color>";
        if (dexReqFufil)
            dexterityReq = "<color=green>" + skillStatRequire.dexterity + "</color>";
        else
            dexterityReq = "<color=red>" + skillStatRequire.dexterity + "</color>";
        if (agiReqFufil)
            agilityReq = "<color=green>" + skillStatRequire.agility + "</color>";
        else
            agilityReq = "<color=red>" + skillStatRequire.agility + "</color>";
        if (lucReqFufil)
            luckReq = "<color=green>" + skillStatRequire.luck + "</color>";
        else
            luckReq = "<color=red>" + skillStatRequire.luck + "</color>";
        if (intReqFufil)
            magpowReq = "<color=green>" + skillStatRequire.magicPow + "</color>";
        else
            magpowReq = "<color=red>" + skillStatRequire.magicPow + "</color>";

        return "" + extraSkill.name + "\n" + "\n" +
            power + "\n" + "\n" +
            scope + "\n" +
            inflict + "\n" +
            accuracy + "\n" + "\n" +
            "Cost: " + extraSkill.cost + " SP\n" + "\n" +
            "Strength: " + strengthReq + "\n" +
            "Vitality: " + vitalityReq + "\n" +
            "Luck: " + luckReq + "\n" +
            "Dexterity: " + dexterityReq + "\n" +
            "Agility: " + agilityReq + "\n" +
            "Intelligence: " + magpowReq + "\n";
    }
    public string DisplayAbilityReq(S_Move ability, O_BattleCharacter bcDat) {
        S_Move extraSkill = ability;
        S_Stats skillStatRequire = extraSkill.statReq;

        bool strReqFufil = bcDat.strength >= skillStatRequire.strength;
        bool vitReqFufil = bcDat.vitality >= skillStatRequire.vitality;
        bool dexReqFufil = bcDat.dexterity >= skillStatRequire.dexterity;
        bool agiReqFufil = bcDat.agility >= skillStatRequire.agility;
        bool lucReqFufil = bcDat.luck >= skillStatRequire.luck;
        bool intReqFufil = bcDat.magicPow >= skillStatRequire.magicPow;

        bool canEquip = strReqFufil && vitReqFufil && dexReqFufil && agiReqFufil && lucReqFufil && intReqFufil;

        string strengthReq = "";
        string vitalityReq = "";
        string dexterityReq = "";
        string agilityReq = "";
        string luckReq = "";
        string magpowReq = "";

        if (strReqFufil)
        {
            if (skillStatRequire.strength != 0)
                strengthReq = "<color=green>" + skillStatRequire.strength + "</color>";
        }
        else
            strengthReq = "<color=red>" + skillStatRequire.strength + "</color>";
        if (vitReqFufil)
        {
            if (skillStatRequire.vitality != 0)
                vitalityReq = "<color=green>" + skillStatRequire.vitality + "</color>";
        }
        else
            vitalityReq = "<color=red>" + skillStatRequire.vitality + "</color>";
        if (dexReqFufil)
            dexterityReq = "<color=green>" + skillStatRequire.dexterity + "</color>";
        else
            dexterityReq = "<color=red>" + skillStatRequire.dexterity + "</color>";
        if (agiReqFufil)
            agilityReq = "<color=green>" + skillStatRequire.agility + "</color>";
        else
            agilityReq = "<color=red>" + skillStatRequire.agility + "</color>";
        if (lucReqFufil)
            luckReq = "<color=green>" + skillStatRequire.luck + "</color>";
        else
            luckReq = "<color=red>" + skillStatRequire.luck + "</color>";
        if (intReqFufil)
            magpowReq = "<color=green>" + skillStatRequire.magicPow + "</color>";
        else
            magpowReq = "<color=red>" + skillStatRequire.magicPow + "</color>";

        return "" + extraSkill.name + "\n" +
            "Strength: " + strengthReq + "\n" +
            "Vitality: " + vitalityReq + "\n" +
            "Luck: " + luckReq + "\n" +
            "Dexterity: " + dexterityReq + "\n" +
            "Agility: " + agilityReq + "\n" +
            "Intelligence: " + magpowReq + "\n";
    }
}
