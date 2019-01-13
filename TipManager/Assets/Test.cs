using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private int index = 0;

    void Start()
    {
        var pos = GameObject.Find("UI Root/Camera");
        GameObject.Instantiate(Resources.Load<GameObject>("FloatTipText"), pos.transform).name = "FloatTipText";
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(400, 300, 100, 50), "textButton"))
        {
            TipManager.AddTip(":)GameTip" + index);
            index++;
        }
    }
}
