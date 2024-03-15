using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Registers/Move")]
public class R_Move : R_Default
{
    public S_Move move;

    public void SetMove(S_Move newMove) {
        move = newMove;
    }
}

