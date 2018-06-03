using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardLine
{
    public ScoreboardLine(string name, float score)
    {
        m_playerName = name;
        m_score = score;
    }

    public string m_playerName;
    public float m_score;
}

public class Scoreboard
{
    public List<ScoreboardLine> m_aLines;

    public Scoreboard(string dataFromWeb)
    {
        m_aLines = new List<ScoreboardLine>();
        string[] splited = dataFromWeb.Split(';');
        foreach(string split in splited)
        {
            string[] datas = split.Split(':');
            if (datas.Length != 2)
                continue;

            float score;
            if (float.TryParse(datas[1], out score))
                m_aLines.Add(new ScoreboardLine(datas[0], score));
        }
    }

    public override string ToString()
    {
        string str = "";
        foreach(ScoreboardLine line in m_aLines)
        {
            str += line.m_playerName + " : " + line.m_score + "\n";
        }
        return str;
    }
}

public class ScoreManager : MonoBehaviour
{
    float m_timeElapsed = 0;
    bool m_started = false;

    string m_playerName = "";

    string m_key = "YGM_6zef45z";

    Scoreboard m_scoreboard;

    [SerializeField]
    Text m_timeText;
    [SerializeField]
    Text m_timeBonusText;

    // Load dynamicly the online scoreboard
    IEnumerator Start()
    {
        using (WWW www = new WWW("http://jordan-bas.com/admin/scores/" + m_key))
        {
            yield return www;
            m_scoreboard = new Scoreboard(www.text);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if(m_started)
        {
            m_timeElapsed += Time.deltaTime;
        }

        int minutes = (int)(m_timeElapsed / 60);
        int seconds = (int) m_timeElapsed - minutes * 60;

        int minutesBonus = (int)(ScoreMailbox.s_score / 60);
        int secondsBonus = (int)(ScoreMailbox.s_score - minutesBonus * 60);

        m_timeText.text = "" + minutes + " : " + seconds;
        m_timeBonusText.text = "" + minutesBonus + " : " + secondsBonus;
    }

    public void StartTimer()
    {
        m_started = true;
    }

    public void StopTimer()
    {
        m_started = false;
    }

    public float GetTimeScore()
    {
        return m_timeElapsed - ScoreMailbox.s_score;
    }

    public void UploadScore()
    {
        if(m_playerName == "")
        {
            Debug.LogError("Can't upload score : playername incorrect");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("key", m_key);
        form.AddField("player", m_playerName);
        form.AddField("score", "!"+GetTimeScore().ToString()+"!");
        WWW www = new WWW("http://jordan-bas.com/admin/add_scores/", form);

        StartCoroutine(SendScore(www));

    }

    public Scoreboard GetScores()
    {
        return m_scoreboard;
    }

    public void SetPlayerName(string name)
    {
        m_playerName = name.TrimEnd().TrimStart();
    }

    IEnumerator SendScore(WWW www)
    {
        yield return www;

         // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
