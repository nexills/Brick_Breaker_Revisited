using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    // this script is used to store brick properties
    public int worth;
    public int coin;
    public bool spawning;

    // default values for worth and coin
    void Start() {
        if (worth == 0) worth = 10;
        if (coin == 0) coin = 1;
    }
}
