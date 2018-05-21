using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    float m_timeElapsed = 0;
    bool m_started = false;

    string m_playerName = "";

    string m_key = "";

	// Update is called once per frame
	void Update ()
    {
        if(m_started)
        {
            m_timeElapsed += Time.deltaTime;
        }
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

        
        
    }

    public List<string> GetScores()
    {
        List<string> aScores = new List<string>();

        WWW www = new WWW("http://jordan-bas.com/scores/" + m_key + "/all/");

        return aScores;

    }

    public void SetPlayerName(string name)
    {
        m_playerName = name.TrimEnd().TrimStart();
    }
}
