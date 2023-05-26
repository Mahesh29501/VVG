using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    private ScoreData sd;

    void Awake()
    {
        var json = PlayerPrefs.GetString("scores", "{}");
        sd = JsonUtility.FromJson<ScoreData>(json);
    }
    
    public IEnumerable<Score> GetHighScore()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }
    public void AddScore(Score score)
    {
        Debug.Log(score);
        sd.scores.Add(score);
    }

    public void OnDestroy()
    {
        //SaveScore();
        //PlayerPrefs.DeleteAll();
    }

    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }

}
