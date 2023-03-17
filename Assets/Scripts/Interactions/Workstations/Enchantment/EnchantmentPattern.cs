using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantmentPattern : MonoBehaviour
{
    [SerializeField] public PatternPoint[] points = new PatternPoint[7];
    //[SerializeField] public PatternLine[] lines = new PatternLine[18];
    [SerializeField] private Logic logic;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void turnOffPoints()
    {
        foreach(PatternPoint p in points){
            p.turnOff();
        }
    }

    public void turnOffLines()
    {
        foreach (PatternLine l in logic.lines)
        {
            l.turnOff();
        }
    }
    
    public void resetRunes()
    {
        for (int i = 0; i < logic.runes.Count; i++)
        {
            logic.runes[i].drawnLines = 0;

        }
    }
    public void resetLogic()
    {
        logic.resetLogic();
    }
    public bool checkEnchantment()
    {
        if(logic.enchantmentRune is not null)
        {
            return true;
        }
        return false;
    }
}
