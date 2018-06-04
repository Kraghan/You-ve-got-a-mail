using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextHandler : MonoBehaviour {
    
    Text m_text;

    private void Start()
    {
        m_text = GetComponent<Text>();
    }

    public void WriteLetter(string letter)
    {
        m_text.text += letter;
    }

    public void RemoveLetter()
    {
        if(m_text.text != "")
            m_text.text = m_text.text.Remove(m_text.text.Length - 1);
    }
}
