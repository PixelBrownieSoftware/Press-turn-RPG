using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Move list")]
public class R_MoveList : R_Default
{
    public List<S_Move> moveListRef = new List<S_Move>();

    private void OnDisable()
    {
        if (_isReset)
        {
            moveListRef.Clear();
        }
    }

    public S_Move PickRandom() {
        return moveListRef[Random.Range(0, moveListRef.Count)];
    }

    public void Clear() {
        moveListRef.Clear();
    }

    public void SetMoves(List<S_Move> moves)
    {
        moveListRef= moves;
    }
    public void AddMoves(List<S_Move> moves) {
        moveListRef.AddRange(moves);
    }

    public void AddMove(S_Move mov) {
        moveListRef.Add(mov);
    }

    public bool ListContains(S_Move targ)
    {
        return moveListRef.Contains(targ);
    }

    public void RemoveMove(S_Move targ) { 
        moveListRef.Remove(targ);
    }
    public S_Move GetMove(string index)
    {
        return moveListRef.Find(x => x.name == index);
    }

    public S_Move GetMove(int index)
    {
        return moveListRef[index];
    }
}
