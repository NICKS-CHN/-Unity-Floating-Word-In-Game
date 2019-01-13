using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TipManager
{
    private static FloatTipText tipTool;

    public static void AddTip(string tip)
    {
        if (tipTool == null)
            tipTool = GameObject.Find("UI Root/Camera/FloatTipText").GetComponent<FloatTipText>();

        tipTool.Add(tip, Color.white, 0.1f);
    }
}
