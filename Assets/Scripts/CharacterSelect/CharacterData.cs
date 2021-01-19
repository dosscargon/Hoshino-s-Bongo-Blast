using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CharacterData {
    const string DirectoryName = "Clappers";
    const string DefaultID = "Hoshino";

    public string CharacterID {
        get;
        private set;
    }

    public Sprite Body;

    public List<Sprite> LeftArm = new List<Sprite>();
    public List<Sprite> RightArm = new List<Sprite>();

    public CharacterData(string characterID) {
        CharacterID = characterID;
        Body = LoadSprite(characterID, "body");
        LeftArm.Add(LoadSprite(characterID, "left0"));
        LeftArm.Add (LoadSprite(characterID, "left1"));
        RightArm.Add( LoadSprite(characterID, "right0"));
        RightArm.Add(LoadSprite(characterID, "right1"));
    }

    private Sprite LoadSprite(string characterID,string spriteName) {
        Sprite sprite = (Sprite)Resources.Load($"{DirectoryName}\\{characterID}\\{spriteName}", typeof(Sprite));

        if (sprite == null) {
            sprite = (Sprite)Resources.Load($"{DirectoryName}\\{DefaultID}\\{spriteName}", typeof(Sprite));
        }

        return sprite;
    }
}
