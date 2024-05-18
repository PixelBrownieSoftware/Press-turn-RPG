using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "System/Battle group member Channel")]
public class CH_BattleGroupMember : SO_ChannelDefaut
{
    public UnityAction<S_BattleGroup.S_GroupMember> OnGroupMemberEvent;
    public void RaiseEvent(S_BattleGroup.S_GroupMember member)
    {
        if (OnGroupMemberEvent != null)
            OnGroupMemberEvent.Invoke(member);
    }
}
