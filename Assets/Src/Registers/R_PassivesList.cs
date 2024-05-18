using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Passives list")]
public class R_PassivesList : R_Default
{
    public List<S_Passive> passiveListRef = new List<S_Passive>();

    private void OnDisable()
    {
        if (_isReset)
        {
            passiveListRef.Clear();
        }
    }

    public S_Passive PickRandom()
    {
        return passiveListRef[Random.Range(0, passiveListRef.Count)];
    }

    public void Clear()
    {
        passiveListRef.Clear();
    }

    public void SetPassives(List<S_Passive> passives)
    {
        passiveListRef = passives;
    }
    public void AddPassives(List<S_Passive> passives)
    {
        passiveListRef.AddRange(passives);
    }

    public void AddPassive(S_Passive passive)
    {
        passiveListRef.Add(passive);
    }

    public bool ListContains(S_Passive targ)
    {
        return passiveListRef.Contains(targ);
    }

    public void RemovePassive(S_Passive targ)
    {
        passiveListRef.Remove(targ);
    }
    public S_Passive GetPassive(string index)
    {
        return passiveListRef.Find(x => x.name == index);
    }

    public S_Passive GetPassive(int index)
    {
        return passiveListRef[index];
    }
}
