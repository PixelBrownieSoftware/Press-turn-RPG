using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using System.Reflection;
using System.Threading;
using Unity.VisualScripting;

public class S_ExperienceMenu : S_MenuSystem
{
    public R_BattleCharacterList players;
    public R_BattleCharacterList enemies;
    public R_BattleCharacterList goneEnemies;
    public TextMeshProUGUI[] expText;
    public Slider[] sliders;
    public GameObject[] expGUI;
    public CH_Text changeMenu;
    public CH_Func overworldButtonFunc;

    public B_Function continueButton;
    public B_Function overworldButton;
    [SerializeField]
    public CH_MapTransfer mapTrans;
    [SerializeField]
    public CH_SoundPitch soundPlayer;
    public AudioClip expGainSound;
    public AudioClip levelUpSound;

    [SerializeField]
    private Dictionary<O_BattleCharacter, float> playerExpDist = new Dictionary<O_BattleCharacter, float>();
    private int expAnimationCoroutines = 0;
    private float[] expBefore;

    private void OnEnable()
    {
        overworldButtonFunc.OnFunctionEvent += SwtichToOverworld;
    }

    private void OnDisable()
    {
        overworldButtonFunc.OnFunctionEvent -= SwtichToOverworld;
    }

    public override void StartMenu()
    {
        base.StartMenu();
        overworldButton.gameObject.SetActive(false);
        //continueButton.gameObject.SetActive(true);
        foreach (var gui in expGUI)
        {
            gui.gameObject.SetActive(false);
        }
        int index = 0;
        foreach (var chara in players.battleCharList)
        {
            expGUI[index].gameObject.SetActive(true);
            //expText[index].text = chara.name + " LV: " + chara.level + " EXP: " + (exp[index] * 100) + "%";
            sliders[index].value = chara.experiencePoints / 100;
        }
        StartCoroutine(ExpereinceUIUpadate());
    }

    public List<float> CalculateExp() {
        List<float> leveltoFinalExp = new List<float>();
        playerExpDist.Clear();
        expBefore = new float[players.battleCharList.Count];
        List<O_BattleCharacter> expCharcters = new List<O_BattleCharacter>();
        expCharcters.AddRange(enemies.battleCharList);
        expCharcters.AddRange(goneEnemies.battleCharList);
        int index = 0;
        foreach (var player in players.battleCharList)
        {
            expBefore[index] = player.experiencePoints;
            float total = 0;
            foreach (var enemy in expCharcters)
            {
                float expYeild = (((float)enemy.level / (float)player.level) * 0.35f) * 100f;
                //print("Level descrepancy " + ((float)enemy.level / player.level));
                total += expYeild;
            }
            if (!playerExpDist.ContainsKey(player))
            {
                playerExpDist.Add(player, total);
            }
            print(player.name + " gained " + total + " exp.");
            leveltoFinalExp.Add(total);
            index++;
        }

        foreach (var pl in playerExpDist) {
            float remaining = pl.Value;
            while (remaining > 0) {
                remaining = pl.Key.ExperiencePointsCalculation(remaining);
                if (remaining > 0)
                {
                    remaining /= 2;     //If the user gains 200% exp, they won't go up 2 levels it'll only be 150% in reality
                }
            }
        }
        return leveltoFinalExp;
    }

    public IEnumerator ExpereinceUIUpadate()
    {
        yield return new WaitForSeconds(0.1f);
        List<float> leveltoFinalExp = CalculateExp();
        print(leveltoFinalExp.Count);
        for (int i = 0; i < leveltoFinalExp.Count; i++)
        {
            expAnimationCoroutines++;
            StartCoroutine(LevelUpAnim(leveltoFinalExp[i], i));
        }
        while (expAnimationCoroutines > 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.01f);
        leveltoFinalExp.Clear();
        overworldButton.gameObject.SetActive(true);
    }

    public IEnumerator LevelUpAnim(float expGainedTotal, int i) {
        int finalExp = Mathf.RoundToInt(expGainedTotal);
        print("Final exp: " + finalExp);
        int startingPoint = Mathf.RoundToInt(expBefore[i]);

        for (int exp = startingPoint; exp != finalExp + 1; exp++)
        {
            soundPlayer.RaiseEvent(expGainSound, (exp / 100f) * 3.8f);
            expGUI[i].gameObject.SetActive(true);
            expText[i].text = players.GetIndex(i).name + " EXP: " + exp + "%";
            sliders[i].value = (exp % 100) / 100f;
            yield return new WaitForSeconds(0.06f);
            if (exp % 100 == 0 && exp != 0)
            {
                soundPlayer.RaiseEvent(levelUpSound, 1);
            }
        }

        yield return new WaitForSeconds(0.01f);
        print("Done!");
        expAnimationCoroutines--;
    }

    public void SwtichToOverworld()
    {
        players.Clear();
        mapTrans.RaiseEvent("Hubworld");
    }

    public void SetbattleStatsAfterExp()
    {
        continueButton.gameObject.SetActive(false);
        overworldButton.gameObject.SetActive(true);
        int index = 0;
        foreach (var chara in players.battleCharList)
        {
            expGUI[index].gameObject.SetActive(true);
            //exp[index] = chara.experiencePoints;
            //expText[index].text = chara.name + " LV: " + chara.level + " EXP: " + (exp[index] * 100) + "%";
            sliders[index].value = chara.experiencePoints / 1;
        }
    }
}
