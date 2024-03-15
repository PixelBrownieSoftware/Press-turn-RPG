using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Save_Data {
    public Save_BattleCharacter[] battleCharacters;
    public Save_Data(R_BattleCharacterList characterListRef) {
        O_BattleCharacter[] battleCharacters = characterListRef.battleCharList.ToArray();
        this.battleCharacters = new Save_BattleCharacter[battleCharacters.Length];
        for (int i=0; i < battleCharacters.Length; i++) {
            O_BattleCharacter battleCharacter = battleCharacters[i];
            this.battleCharacters[i] = new Save_BattleCharacter(battleCharacter);
        }
    }
}

public class S_PartySaver : MonoBehaviour
{
    public void SavePartyData(R_BattleCharacterList characterListRef) {
        Save_Data data = new Save_Data(characterListRef);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void LoadPartyData()
    {
    }
}