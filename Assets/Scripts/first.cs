using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class first : MonoBehaviour
{
    public int outputValue = 0;
    
    public string[] entries;

    public List<int> answers = new List<int>();

    bool hasAnswer;
    
    private void Start()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        FileStream fs = new FileStream(Application.dataPath + "/scripts/firstinput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, "\r\n?|\n", RegexOptions.Singleline);

        while (!hasAnswer)
        {
            for (int i = 0; i < entries.Length; i++)
            {
                // Method A does not encounter a double
                outputValue += int.Parse(entries[i]);
                UpdateList(outputValue);
                if (hasAnswer) break;
            }
        }

        st.Stop();
        // first run A: https://gyazo.com/f5531be594d8e7eee6d8204e38dd9ffd
        // first run B: https://gyazo.com/803d497ad1c6ea13b4b65c9b7aba261d
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
        UnityEngine.Debug.Log("Answer: " + outputValue);
    }

    void UpdateList(int value)
    {
        if (answers.Contains(value))
        {
            UnityEngine.Debug.Log("Double value: " + outputValue);
            hasAnswer = true;
        }
        else
        {
            answers.Add(value);
        }
    }
}
