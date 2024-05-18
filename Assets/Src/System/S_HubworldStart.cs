using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_HubworldStart : MonoBehaviour
{
    public S_BattleGroup partyMembersStart;
    public R_BattleCharacterList partyMembers;
    public CH_BattleGroupMember bcFactoryInput;
    public R_BattleCharacter bcFactoryOutput;
    //public S_RPGGlobals rpgManager;
    [SerializeField]
    private R_Boolean isSave;
    [SerializeField]
    private R_Boolean hasStartedGame;
    public R_Boolean skillsReadonly;
    public CH_Func onLoad;

    private void OnEnable()
    {
        onLoad.OnFunctionEvent += SceneInitialise;
    }
    private void OnDisable()
    {
        onLoad.OnFunctionEvent -= SceneInitialise;
    }

    void SceneInitialise()
    {

        skillsReadonly.boolean = true;
        if (!hasStartedGame.boolean)
        {
            if (!isSave.boolean)
            {
                foreach (var ind in partyMembersStart.members_Player)
                {
                    print(ind.memberDat.name);
                    bcFactoryInput.RaiseEvent(ind);
                    partyMembers.Add(bcFactoryOutput.battleCharacter);
                }
                /*
                foreach (var ind in groupsStart.groupList)
                {
                    //rpgManager.groupsCurrent.AddGroup(ind);
                }
                */
            }
            else
            {
                //rpgManager.LoadSaveData();
            }
            hasStartedGame.boolean = true;
        }
    }
}
