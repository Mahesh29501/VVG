using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowui;
    public ScoreManager scoreManager;

    private void Start()
    {
        var scores = scoreManager.GetHighScore().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowui, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}
