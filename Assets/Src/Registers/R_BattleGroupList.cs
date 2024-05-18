using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Battle group list")]
public class R_BattleGroupList : R_Default
{
    public List<S_BattleGroup> groupList = new List<S_BattleGroup>();
    [SerializeField]
    private List<S_BattleGroup> groupListDefault = new List<S_BattleGroup>();

    private void OnEnable()
    {
        if (_isReset)
        {
            groupList.AddRange(groupListDefault);
        }
    }

    private void OnDisable()
    {
        if (_isReset)
        {
            groupList.Clear();
        }
    }

    public S_BattleGroup GetGroup(string group) {
        return groupList.Find(x => x.name == group);
    }
    public void RemoveGroup(S_BattleGroup group)
    {
        groupList.Remove(group);
    }

    public void Clear() {
        groupList.Clear();
    }

    public void AddGroup(S_BattleGroup group) {
        if (groupList == null)
            groupList = new List<S_BattleGroup>();
        groupList.Add(group);
    }
}
