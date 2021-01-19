using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour {
    public List<Sprite> sprites;
    
    public int spriteNumber;
    //public int SpriteNumber {
    //    get { return spriteNumber; }
    //    set {
    //        spriteNumber = value % sprites.Count;

    //        GetComponent<Image>().sprite = sprites[value];

    //        Debug.Log($"changed:{SpriteNumber}");
    //    }
    //}

    void OnValidate() {
    }

    // Start is called before the first frame update
    void Start() {
        //spriteNumber = 0;
    }

    int _spriteNumber_old = 0;
    // Update is called once per frame
    void Update() {
        if (spriteNumber != _spriteNumber_old) {
            spriteNumber = spriteNumber % sprites.Count;

            GetComponent<Image>().sprite = sprites[spriteNumber];
        }

        _spriteNumber_old = spriteNumber;
    }
}
