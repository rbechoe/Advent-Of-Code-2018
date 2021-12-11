using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class ten : MonoBehaviour
{
    public string[] entries;

    public List<Vector3> positions;
    public List<Vector3> velocities;
    public List<GameObject> objects;

    public GameObject sampleObj;

    public bool animating;

    void Start()
    {
        StartCoroutine(Brrr());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && !animating)
        {
            StartCoroutine(DoingAnimation());
            UnityEngine.Debug.Log("manual update");
        }
    }

    IEnumerator Brrr()
    {
        animating = true;
        Stopwatch st = new Stopwatch();
        st.Start();

        FileStream fs = new FileStream(Application.dataPath + "/scripts/teninput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, "\r\n?|\n", RegexOptions.Singleline);

        // setup data
        for (int i = 0; i < entries.Length; i++)
        {
            // first match pos   scnd match velo
            // position=< 7,  6> velocity=<-1, -1>
            Match xMatches = Regex.Match(entries[i], "<.*?,");
            Match yMatches = Regex.Match(entries[i], ",.*?>");
            int xpos = int.Parse(xMatches.ToString().Replace("<", "").Replace(",", ""));
            int xspeed = int.Parse(xMatches.NextMatch().ToString().Replace("<", "").Replace(",", "")); // left is negative, right is positive

            int ypos = int.Parse(yMatches.ToString().Replace(",", "").Replace(">", ""));
            int yspeed = int.Parse(yMatches.NextMatch().ToString().Replace(",", "").Replace(">", "")); // up = negative, down is positive

            GameObject newObj = Instantiate(sampleObj, Vector3.zero, Quaternion.identity);
            Vector3 pos = new Vector3(xpos, ypos, 0);
            Vector3 velo = new Vector3(xspeed, yspeed, 0);
            newObj.transform.position = pos;

            objects.Add(newObj);
            positions.Add(pos);
            velocities.Add(velo);
        }

        // I timed the stopwatch first at this exact line, before the animations hence the short time
        // first run A https://gyazo.com/a5aafd3ae4ee83e997611038ab9a7397
        // 10086 updates for https://gyazo.com/161c112e61ef9ed353760a1cba6ac966 OH SHIT THIS WAS PART TWO!!

        // visualize data x amount of times with waiting times for answer since this will use wait for seconds it is outside of stopwatch
        for (int i = 0; i < objects.Count; i++)
        {
            positions[i] += velocities[i] * 10086;
            objects[i].transform.position = positions[i];
        }

        animating = false;

        // Run A https://gyazo.com/4d915d65af149b42a5748a63b489f7d1
        // Run B https://gyazo.com/9cf4517caafc4024bb455dad9016286f
        // Run C https://gyazo.com/246d881d3db5c3eceae23db86b0edb31
        // Run D https://gyazo.com/c7177346ad922cc7df88e541c5de86ad
        st.Stop();
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
        yield return new WaitForEndOfFrame();
    }

    IEnumerator DoingAnimation()
    {
        animating = true;
        for (int j = 0; j < objects.Count; j++)
        {
            positions[j] += velocities[j];
            objects[j].transform.position = positions[j];
        }
        yield return new WaitForEndOfFrame();
        animating = false;
    }
}
