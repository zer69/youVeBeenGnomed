using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune
{
    
    public string runeName;
    public int enchantmentId;
    public List<PatternLine> lines = new List<PatternLine>();
    public int drawnLines = 0;


    public bool drawnCheck()
    {
        //Debug.Log("check rune " + runeName);



        //Debug.Log(drawnLines + " == " + lines.Count);
        if (drawnLines == lines.Count)
        {
            return true;
        }

        return false;
    }
}
