using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Toggle : MonoBehaviour
{
    UnityEngine.UI.Toggle toggle;
    // Start is called before the first frame update
    void Start() {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });
        Info.double_paddle = true;
    }

    // Update is called once per frame
    void Update() {

    }

    void ToggleValueChanged(bool isToggle) {
        Info.double_paddle = !Info.double_paddle;
    }
}
