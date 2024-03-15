using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Enemy group list")]
public class R_EnemyGroupList : R_Default
{
    public List<S_EnemyGroup> groupList = new List<S_EnemyGroup>();
    [SerializeField]
    private List<S_EnemyGroup> groupListDefault = new List<S_EnemyGroup>();

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

    public S_EnemyGroup GetGroup(string group) {
        return groupList.Find(x => x.name == group);
    }
    public void RemoveGroup(S_EnemyGroup group)
    {
        groupList.Remove(group);
    }

    public void Clear() {
        groupList.Clear();
    }

    public void AddGroup(S_EnemyGroup group) {
        if (groupList == null)
            groupList = new List<S_EnemyGroup>();
        groupList.Add(group);
    }
}
