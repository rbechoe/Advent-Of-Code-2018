using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class thirteen : MonoBehaviour
{
    public string[] entries;
    Dictionary<string, int> typeCollection = new Dictionary<string, int>();

    Dictionary<string, int> map = new Dictionary<string, int>();

    List<Node> nodes = new List<Node>();

    List<Minecart> carts = new List<Minecart>();
    public string[] strings;

    void Start()
    {
        FileStream fs = new FileStream(Application.dataPath + "/scripts/thirteeninput.txt", FileMode.Open);
        string content = "";
        using (StreamReader read = new StreamReader(fs, true))
        {
            content = read.ReadToEnd();
        }
        entries = Regex.Split(content, "\n", RegexOptions.Singleline);

        StartCoroutine(Brrr());
    }


    IEnumerator Brrr()
    {
        Stopwatch st = new Stopwatch();
        st.Start();

        // minecarts > < v ^
        // tracks | - / \ +
        // + logic: 1st left, 2nd straight, 3rd right... repeat
        typeCollection.Add(">", 0); //cart - right
        typeCollection.Add("<", 1); //cart - left
        typeCollection.Add("^", 2); //cart - up
        typeCollection.Add("v", 3); //cart - down
        typeCollection.Add("|", 4);
        typeCollection.Add("-", 4);
        typeCollection.Add("/", 5);
        typeCollection.Add("x", 6);
        typeCollection.Add("+", 7);
        typeCollection.Add(" ", 8);

        // fill map
        int count = 0;
        UnityEngine.Debug.Log(entries.Length);
        UnityEngine.Debug.Log(entries[0].Length);
        for (int y = 0; y < entries.Length; y++)
        {
            for (int x = 0; x < entries[y].Length; x++)
            {
                // minecarts start always on straight paths?
                string symbol = entries[y].ToCharArray()[x].ToString();

                if (symbol == @"\") symbol = "x";
                int item = 8;
                try
                {
                    item = typeCollection[symbol]; // map carts and tracks to value
                }
                catch { }
                if (item < 4)
                {
                    Minecart newCart = new Minecart(x, y);
                    newCart.direction = item;
                    carts.Add(newCart);
                    item = 4;
                }
                map.Add(x + " - " + y, item);
                count++;
            }
        }
        strings = map.Keys.ToArray();

        // update all minecarts
        for (int i = 0; i < 100; i++)
        {
            foreach (Minecart cart in carts)
            {
                // if cart is on special track update direction
                UnityEngine.Debug.Log(cart.position + " -- " + cart.direction);
                // TODO somehow bugs off the track
                int trackPiece = map[cart.position.x + " - " + cart.position.y];
                if (trackPiece != 4 && trackPiece != 7)
                {
                    // cart was moving to the right
                    if (cart.direction == 0 && trackPiece == 5)
                    {
                        cart.direction = 2;
                    }
                    else if (cart.direction == 0 && trackPiece == 6)
                    {
                        cart.direction = 3;
                    }
                    // cart was moving to the left
                    else if (cart.direction == 1 && trackPiece == 5)
                    {
                        cart.direction = 3;
                    }
                    else if (cart.direction == 1 && trackPiece == 6)
                    {
                        cart.direction = 2;
                    }
                    // cart was moving to the up
                    else if (cart.direction == 2 && trackPiece == 5)
                    {
                        cart.direction = 0;
                    }
                    else if (cart.direction == 2 && trackPiece == 6)
                    {
                        cart.direction = 1;
                    }
                    // cart was moving to the down
                    else if (cart.direction == 2 && trackPiece == 5)
                    {
                        cart.direction = 1;
                    }
                    else if (cart.direction == 2 && trackPiece == 6)
                    {
                        cart.direction = 0;
                    }
                }
                else if (trackPiece == 7)
                {
                    switch (cart.crossCount)
                    {
                        case 0:
                            // cart was moving to the right
                            if (cart.direction == 0)
                            {
                                cart.direction = 2;
                            }
                            // cart was moving to the left
                            else if (cart.direction == 1)
                            {
                                cart.direction = 3;
                            }
                            // cart was moving to the up
                            else if (cart.direction == 2)
                            {
                                cart.direction = 1;
                            }
                            // cart was moving to the down
                            else if (cart.direction == 3)
                            {
                                cart.direction = 0;
                            }
                            break;

                        case 2:
                            // cart was moving to the right
                            if (cart.direction == 0)
                            {
                                cart.direction = 3;
                            }
                            // cart was moving to the left
                            else if (cart.direction == 1)
                            {
                                cart.direction = 2;
                            }
                            // cart was moving to the up
                            else if (cart.direction == 2)
                            {
                                cart.direction = 0;
                            }
                            // cart was moving to the down
                            else if (cart.direction == 3)
                            {
                                cart.direction = 1;
                            }
                            break;
                    }

                    cart.crossCount++;
                    if (cart.crossCount > 2) cart.crossCount = 0;
                }

                // move cart with direction
                switch (cart.direction) // 0 right, 1 left, 2 up, 3 down
                {
                    case 0:
                        cart.position += Vector2Int.right;
                        break;

                    case 1:
                        cart.position += Vector2Int.left;
                        break;

                    case 2:
                        cart.position -= Vector2Int.up;
                        break;

                    case 3:
                        cart.position -= Vector2Int.down;
                        break;
                }
            }

            // check if each cart has collision
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach (Minecart cart in carts)
            {
                if (!positions.Contains(cart.position))
                {
                    positions.Add(cart.position);
                }
                else
                {
                    UnityEngine.Debug.Log("Collision at: " + cart.position);
                }
            }
        }

        st.Stop();
        // first run 
        UnityEngine.Debug.Log(string.Format("took {0} ms to complete", st.ElapsedMilliseconds));
        yield return new WaitForEndOfFrame();
    }
}

public class Minecart
{
    public int direction = 0; // 0 right, 1 left, 2 up, 3 down
    public Vector2Int position;
    public int crossCount = 0; // 0 left, 1 straight (up/down), 2 right

    public Minecart(int x, int y)
    {
        position = new Vector2Int(x, y);
    }
}