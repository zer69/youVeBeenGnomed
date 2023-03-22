using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{

    private bool isStart = false;
    private int linesCount = 0;
    private int firstPoint;
    private int secondPoint;
    public Rune enchantmentRune;
    public List<Rune> runes = new List<Rune>(); 

    [SerializeField] public List<PatternLine> lines = new List<PatternLine>();

    private List<List<int>> pointsInLine = new List<List<int>>();

    //index - id line
    //pair of numbers - points
    private int[,] points = {
        {1, 2}, {2, 3}, {3, 4}, {4, 5}, {5, 6}, {6, 1},
        {6, 2}, {2, 4}, {4, 6}, {5, 1}, {1, 3}, {3, 5},
        {0, 1}, {0, 2}, {0, 3}, {0, 4}, {0, 5}, {0, 6}
    };


    void Start()
    {
        for(int i = 0; i < 18; i++){
            List<int> temp = new List<int>();
            temp.Add(points[i, 0]);
            temp.Add(points[i, 1]);
            pointsInLine.Add(temp);
        }


        //rune fire
        Rune fireRune = new Rune();
        fireRune.runeName = "fire";
        fireRune.enchantmentId = 1;
        fireRune.lines.Add(lines[1]);
        fireRune.lines.Add(lines[13]);
        fireRune.lines.Add(lines[17]);
        fireRune.lines.Add(lines[4]);
        runes.Add(fireRune);

        //rune light
        Rune lightRune = new Rune();
        lightRune.runeName = "light";
        lightRune.enchantmentId = 5;
        lightRune.lines.Add(lines[5]);
        lightRune.lines.Add(lines[17]);
        lightRune.lines.Add(lines[14]);
        lightRune.lines.Add(lines[1]);
        lightRune.lines.Add(lines[13]);
        lightRune.lines.Add(lines[16]);
        lightRune.lines.Add(lines[3]);
        runes.Add(lightRune);

        //rune dark
        Rune darkRune = new Rune();
        darkRune.runeName = "dark";
        darkRune.enchantmentId = 6;
        darkRune.lines.Add(lines[5]);
        darkRune.lines.Add(lines[0]);
        darkRune.lines.Add(lines[13]);
        darkRune.lines.Add(lines[14]);
        darkRune.lines.Add(lines[2]);
        darkRune.lines.Add(lines[3]);
        runes.Add(darkRune);
    }

    
    void Update()
    {
        
    }

    public void activatePoint(int point)
    {
        if (!isStart)
        {
            isStart = true;
            firstPoint = point;
            Debug.Log("firstPoint" + point);
        }
        else
        {

            secondPoint = point;
            Debug.Log("secondPoint" + point);

            if (firstPoint != secondPoint)
            {
                activateLine();
            }
        }
    }

    public void activateLine()
    {
        //need checks
        List<int> line = pointsInLine.FindAll(x => x.Contains(firstPoint)).Find(x => x.Contains(secondPoint));
        Debug.Log("activateLine ");
        if (pointsInLine.Contains(line))
        {
            Debug.Log("find line");
            int lineId = pointsInLine.IndexOf(line);
            if (!lines[lineId].isDrawn)
            {
                Debug.Log("line: " + lineId + " - on");
                lines[lineId].turnOn();
                linesCount++;

                runeHasLine(lineId);
                checkRunes();
            }

            firstPoint = secondPoint;
            secondPoint = -1;
        }

        
    }

    public bool runeHasLine(int lineId)
    {

        for (int i = 0; i < runes.Count; i++)
        {
            if (runes[i].lines.Contains(lines[lineId]))
            {
                runes[i].drawnLines++;
            }
        }

        return false;
    }

    public bool checkRunes()
    {
        for (int i = 0; i < runes.Count; i++)
        {
            if (runes[i].drawnCheck() && runes[i].lines.Count == linesCount)
            {
                Debug.Log("rune " + runes[i].runeName + " completed");
                enchantmentRune = runes[i];
                return true;

            }
        }
        return false;

    }

    public void resetLogic()
    {
        isStart = false;
        linesCount = 0;
        firstPoint = -1;
        secondPoint = -1;
        enchantmentRune = null;
    }
}

