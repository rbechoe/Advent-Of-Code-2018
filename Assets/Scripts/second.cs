using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class second : MonoBehaviour
{
    public string[] entries;

    void Start()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        FileStream fs = new FileStream(Application.dataPath + "/scripts/twoinput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, "\r\n?|\n", RegexOptions.Singleline);

        //int answer = PartA(); // gives good answer
        string answer = PartB(); // gives wrong answer with input, but good answer with example

        st.Stop();
        // first run A https://gyazo.com/b6b7a58476058368c2052ad4b3aeb27e
        // first run B https://gyazo.com/25743ea8586b19d25ae6ce00b375e05a
        UnityEngine.Debug.Log("Answer: " + answer);
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
    }

    int PartA()
    {
        int doubleTimes = 0;
        int trippleTimes = 0;
        int answer = 0;
        foreach (string entry in entries)
        {
            Dictionary<string, int> multiples = new Dictionary<string, int>(); // letter, amount

            for (int i = 0; i < entry.Length; i++)
            {
                string key = entry[i].ToString();
                if (multiples.ContainsKey(key))
                {
                    multiples[key]++;
                }
                else
                {
                    multiples.Add(key, 1);
                }
            }

            if (multiples.Values.ToList().Contains(2)) doubleTimes++;
            if (multiples.Values.ToList().Contains(3)) trippleTimes++;
        }
        answer = doubleTimes * trippleTimes;

        return answer;
    }

    string PartB()
    {
        string answer = "";
        string compareA = "";
        string compareB = "";
        int amountMatchingCharacters = 0;

        for (int a = 0; a < entries.Length; a++)
        {
            // store all letters from string A in list
            for (int b = 0; b < entries.Length; b++)
            {
                if (a == b) continue;
                int matchCount = 0;

                for (int i = 0; i < entries[i].Length; i++)
                {
                    if (entries[a][i].ToString() == entries[b][i].ToString()) matchCount++;
                    // TODO can optimize by continuing if we already know that its impossible to improve matchcount
                }

                if (matchCount > amountMatchingCharacters)
                {
                    amountMatchingCharacters = matchCount;
                    compareA = entries[a];
                    compareB = entries[b];
                    UnityEngine.Debug.Log("A: " + compareA);
                    UnityEngine.Debug.Log("B: " + compareB);
                    UnityEngine.Debug.Log("Times: " + amountMatchingCharacters);
                }
            }
        }

        // return the matching characters
        for (int i = 0; i < compareA.Length; i++)
        {
            if (compareA[i] == compareB[i])
            {
                answer = answer + compareA[i];
            }
        }

        return answer;
    }
}
