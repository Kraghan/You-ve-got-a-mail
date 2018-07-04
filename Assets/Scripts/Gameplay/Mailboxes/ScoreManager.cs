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
	float m_timeRemaining = 600;
    bool m_started = false;
	int m_points = 0;
	int s_total = 0;

    string m_playerName = "";

    string m_key = "YGM_6zef45z";

    Scoreboard m_scoreboard;

    [SerializeField]
    public Text m_timeText;
	public Text Time_Remaining;
	public Text Time_Leisure;
    [SerializeField]
    Text m_timeBonusText;
	public Text m_PointsText;
	public Text m_multi_text;
	public RectTransform m_multi_barre;
	public Text m_TotalMailboxes;
	public Mode_selector Mode_selection;
	public Transform All_Mailboxes;
	public VacuumMailBox[] The_Mailboxes;
	public Endings theendings;

    // Load dynamicly the online scoreboard
    IEnumerator Start()
    {
        using (WWW www = new WWW("http://jordan-bas.com/admin/scores/" + m_key))
        {
            yield return www;
            m_scoreboard = new Scoreboard(www.text);
        }

		The_Mailboxes = All_Mailboxes.GetComponentsInChildren<VacuumMailBox> ();

		foreach (VacuumMailBox laboite in The_Mailboxes) {
			if (laboite.tag == "Trigger")
				s_total ++;
		} 
    }

	int GetDigitInNumber (float digit, int number) {
		
		return ((number% (int)Mathf.Pow(10,digit)) / (int)(Mathf.Pow(10, digit-1)));

	}

    // Update is called once per frame
    void Update ()
    {
		//Je fait évoler correctement le multiplicateur de points
		if (ScoreMailbox.m_scoremultiplier > 1)
			ScoreMailbox.multi_timer += Time.deltaTime;

		if (ScoreMailbox.multi_timer >= ScoreMailbox.multi_time) {
			if (ScoreMailbox.m_scoremultiplier > 1) {
				ScoreMailbox.m_scoremultiplier -= 0.5f;
				ScoreMailbox.multi_time += 0.75f;
			}
			ScoreMailbox.multi_timer = 0;
		}	

		//L'affichage du multiplicateur de points
		m_multi_text.text = "x " + ScoreMailbox.m_scoremultiplier;

		//J'affiche la barre à la bonne taille pour le timer de temps
		float scalex = (1f - (ScoreMailbox.multi_timer / ScoreMailbox.multi_time)) * 4f;
		if (ScoreMailbox.m_scoremultiplier == 1)
			scalex = 4;
		m_multi_barre.localScale = new Vector3 (scalex, m_multi_barre.localScale.y, m_multi_barre.localScale.z);

		//L'affichage des points pour le mode points
		m_points = ScoreMailbox.s_scorepoints;

		m_PointsText.text = "" + GetDigitInNumber (9, m_points) + GetDigitInNumber (8, m_points) + GetDigitInNumber (7, m_points) 
			+  " " + GetDigitInNumber (6, m_points) + GetDigitInNumber (5, m_points) + GetDigitInNumber (4, m_points)
			+ " " + GetDigitInNumber (3, m_points) + GetDigitInNumber (2, m_points) + GetDigitInNumber (1, m_points);

		//L'affichage du nombre total de boîtes aux lettres pour le mode ballade moins les 3 du menu
		m_TotalMailboxes.text = "" + ScoreMailbox.s_totalmailbox + " / " + (s_total);

		//Je check si le joueur a trouvé toutes les boîtes aux lettres
		if (Mode_selection.m_defaultPlayMode == Mode_selector.MyPlayMode.LEISURE) {
			if (s_total == ScoreMailbox.s_totalmailbox)
				theendings.LeisureEnding ();
		}

		//L'affichage du temps pour le mode histoire et les autres modes
        if(m_started)
        {
            m_timeElapsed += Time.deltaTime;
        }

		m_timeRemaining = 600 - m_timeElapsed;

		int minutes = 0;
		int seconds = 0;
				
		if (Mode_selection.m_defaultPlayMode != Mode_selector.MyPlayMode.POINTS) {
			minutes = (int)(m_timeElapsed / 60);
			seconds = (int)m_timeElapsed - minutes * 60;
		} else {
			minutes = (int)(m_timeRemaining / 60);
			seconds = (int)m_timeRemaining - minutes * 60;
		}

        int minutesBonus = (int)(ScoreMailbox.s_score / 60);
        int secondsBonus = (int)(ScoreMailbox.s_score - minutesBonus * 60);

		m_timeText.text = "" + Time_Display(minutes, seconds);
		m_timeBonusText.text = "- " + Time_Display(minutesBonus, secondsBonus);
		Time_Remaining.text = "" + Time_Display(minutes, seconds);
		Time_Leisure.text = "" + Time_Display(minutes, seconds);
    }

	string Time_Display (float minutes, float seconds) {
		
		if ((minutes < 10) && (seconds < 10))
			return "0" + minutes + " : 0" + seconds;
		else if (minutes < 10)
			return "0" + minutes + " : " + seconds;
		else if (seconds < 10)
			return "" + minutes + " : 0" + seconds;
		else
			return "" + minutes + " : " + seconds;
		
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

    public float GetTime()
    {
        return m_timeElapsed;
    }

	public string GetTimeText()
	{
		int minutes = (int)(m_timeElapsed / 60);
		int seconds = (int)m_timeElapsed - minutes * 60;

		return "" + Time_Display(minutes, seconds);
	}

	public string GetBonusText()
	{
		int minutesBonus = (int)(ScoreMailbox.s_score / 60);
		int secondsBonus = (int)(ScoreMailbox.s_score - minutesBonus * 60);

		return "- " + Time_Display(minutesBonus, secondsBonus);
	}

	public string GetBalancedText()
	{
		float Balanced_Time = m_timeElapsed - ScoreMailbox.s_score;

		int minutesbalanced = (int)(Balanced_Time / 60);
		int secondsbalanced = (int)(Balanced_Time - minutesbalanced * 60);

		return "" + Time_Display(minutesbalanced, secondsbalanced);
	}

	public string GetPointsText () {

		//L'affichage des points pour le mode points
		m_points = ScoreMailbox.s_scorepoints;

		string The_Points = "" + GetDigitInNumber (9, m_points) + GetDigitInNumber (8, m_points) + GetDigitInNumber (7, m_points) 
			+  " " + GetDigitInNumber (6, m_points) + GetDigitInNumber (5, m_points) + GetDigitInNumber (4, m_points)
			+ " " + GetDigitInNumber (3, m_points) + GetDigitInNumber (2, m_points) + GetDigitInNumber (1, m_points);

		return The_Points;
		
	}
}
