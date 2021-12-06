using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class fourth : MonoBehaviour
{
    public string[] entries;
    Dictionary<int, List<int>> guardNaps = new Dictionary<int, List<int>>(); // ID, array of minutes actually sleeping

    void Start()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        FileStream fs = new FileStream(Application.dataPath + "/scripts/fourinput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, "\r\n?|\n", RegexOptions.Singleline);
        Array.Sort(entries);

        int dictKey = 0;
        for (int i = 0; i < entries.Length; i++)
        {
            Match idMatch = Regex.Match(entries[i], "#.*[0-9]");
            if (idMatch.Success)
            {
                string guardID = string.Join(";", idMatch).Replace("#", "");
                int key = int.Parse(guardID);
                dictKey = key;

                if (!guardNaps.ContainsKey(dictKey)) guardNaps.Add(dictKey, new List<int>());
            }
            else
            {
                Match asleepMatch = Regex.Match(entries[i], "[0-9] ?.(] falls asleep)");
                if (asleepMatch.Success)
                {
                    Match awakeMatch = Regex.Match(entries[i + 1], "[0-9] ?.(] wakes up)");
                    int startTime = int.Parse(string.Join(";", asleepMatch).Replace("] falls asleep", ""));
                    int endTime = int.Parse(string.Join(";", awakeMatch).Replace("] wakes up", ""));

                    // add all minutes asleep
                    for (int j = startTime; j < endTime; j++)
                    {
                        guardNaps[dictKey].Add(j);
                    }
                }
            }
        }

        int highestSleep = 0;
        int id = 0;
        foreach (int guard in guardNaps.Keys)
        {
            int sleepTime = SumSleep(guardNaps[guard]);
            if (sleepTime > highestSleep)
            {
                highestSleep = sleepTime;
                id = guard;
            }
            //UnityEngine.Debug.Log("Guard " + guard + " slept for a total of " + sleepTime + " minutes");
        }
        // Part A
        //UnityEngine.Debug.Log("Guard " + id + " slept the most with " + highestSleep + " minutes");
        //int chosenMinute = MostSleep(guardNaps[id]);
        //UnityEngine.Debug.Log("Chosen minute: " + chosenMinute);
        //int answer = chosenMinute * id;
        //UnityEngine.Debug.Log("Answer: " + answer);

        // Part B
        SleepiestMinute();

        st.Stop();

        // first run A https://gyazo.com/360de8090581d08dbf9fceefac0a1c67
        // first run B https://gyazo.com/0e601ff0e739abca335d580b3b0a1f82
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
    }

    public int SumSleep(List<int> toBeSummed)
    {
        int sum = 0;
        foreach (int item in toBeSummed)
        {
            sum++;
        }
        return sum;
    }

    public int[] minutes;
    public void SleepiestMinute()
    {
        int sleepyGuardId = 0;
        int sleepyMinute = 0;
        int sleepTimes = 0;
        foreach (int guard in guardNaps.Keys)
        {
            // minute used
            for (int i = 0; i < guardNaps[guard].Count; i++)
            {
                // check for matches with minute compared to other minutes and increase amount
                int amount = 0;

                // minutes to check
                for (int j = 0; j < guardNaps[guard].Count; j++)
                {
                    if (i == j) continue;

                    if (guardNaps[guard][i] == guardNaps[guard][j]) amount++;
                }
                
                // update cache after comparing minutes
                if (amount > sleepTimes)
                {
                    sleepyGuardId = guard;
                    sleepTimes = amount;
                    sleepyMinute = guardNaps[guard][i];
                }
            }
        }
        UnityEngine.Debug.Log("Answer: " + sleepyGuardId * sleepyMinute);
    }
    
    public int[] minute;
    public int[] amount;
    public int MostSleep(List<int> sleeps)
    {
        int sleepiestMinute = 0;
        int sleepAmount = 0;
        Dictionary<int, int> sleepTime = new Dictionary<int, int>(); // minute, amount

        foreach (int sleep in sleeps)
        {
            if (!sleepTime.ContainsKey(sleep))
            {
                sleepTime.Add(sleep, 1);
            }
            else
            {
                sleepTime[sleep] += 1;
            }
        }
        minute = sleepTime.Keys.ToArray();
        amount = sleepTime.Values.ToArray();

        foreach(int sleep in sleepTime.Keys)
        {
            if (sleepTime[sleep] > sleepAmount)
            {
                sleepAmount = sleepTime[sleep];
                sleepiestMinute = sleep;
            }
        }

        UnityEngine.Debug.Log("Slept most at minute " + sleepiestMinute + " for " + sleepAmount + " times");

        return sleepiestMinute;
    }
}
