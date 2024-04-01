using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_BattleCharacterActor : MonoBehaviour
{
    public O_BattleCharacter battleCharacter;

    //TODO:
    //Write an event which corresponds with stuff like OnHurt to do animations

    void Update()
    {
        if(battleCharacter != null)
            battleCharacter.position = transform.position;
    }
}
