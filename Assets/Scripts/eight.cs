using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class eight : MonoBehaviour
{
    string[] entries;
    public List<int> intEntries = new List<int>();
    public List<int> metadata = new List<int>();

    List<Node> nodes = new List<Node>();

    void Start()
    {
        FileStream fs = new FileStream(Application.dataPath + "/scripts/eightinput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, " ", RegexOptions.Singleline);

        StartCoroutine(Brrr());
    }

    IEnumerator Brrr()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        for (int i = 0; i < entries.Length; i++)
        {
            intEntries.Add(int.Parse(entries[i]));
        }

        Node firstNode;
        Node currentNode;
        for (int i = 0; i < intEntries.Count; i++)
        {
            // TODO: https://en.wikipedia.org/wiki/Tree_(data_structure) -> generalizations T_T
        }

        st.Stop();
        // first run 
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
        yield return new WaitForEndOfFrame();
    }
}

public class Node
{
    public Node parent;
    public Vector2 header; // quantity child nodes, quantity metadata entries
    public List<Node> childNodes;
    public List<string> metadataEntries;
}
