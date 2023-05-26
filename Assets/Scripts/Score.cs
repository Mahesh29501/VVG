using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Score
{
    public string name;
    public double score;

    public Score(string name, double score)
    {
        this.name = name;
        this.score = score;
    }
}
