using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    private Dictionary<int, Dictionary<Sprite, int>> m_Records = new Dictionary<int, Dictionary<Sprite, int>>();
    private Image[][] m_Reels = new Image[3][];
    public Image[] Reels1 = null;
    public Image[] Reels2 = null;
    public Image[] Reels3 = null;
    public Image[] Reels4 = null;
    public Image[] Reels5 = null;
    public Sprite[] Symbols = null;
    public Button SpinButton = null;

    void Start()
    {
        for (int i = 0; i < m_Reels.Length; i++)
        {
            m_Reels[i] = new Image[5] { Reels1[i], Reels2[i], Reels3[i], Reels4[i], Reels5[i] };
        }

        SpinButton.onClick.AddListener(spinReels);
    }

    private void spinReels()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        m_Records = new Dictionary<int, Dictionary<Sprite, int>>();

        for (int i = 0; i < m_Reels[0].Length; i++)
        {
            for (int j = 0; j < m_Reels.Length; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, Symbols.Length);

                m_Reels[j][i].sprite = Symbols[randomIndex];

                if (m_Records.ContainsKey(i) == false)
                {
                    m_Records[i] = new Dictionary<Sprite, int>();
                }

                if (m_Records[i].ContainsKey(m_Reels[j][i].sprite))
                {
                    m_Records[i][m_Reels[j][i].sprite] += 1;
                }
                else
                {
                    m_Records[i][m_Reels[j][i].sprite] = 1;
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        /*foreach (Image[] reel in m_Reels)
        {
            for (int i = 0; i < reel.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, Symbols.Length);

                reel[i].sprite = Symbols[randomIndex];

                yield return new WaitForSeconds(0.1f);
            }
        }*/

        checkWinCondition();
    }

    private void checkWinCondition()
    {
        // Arrays to store sets for each column
        HashSet<Sprite>[] sets = new HashSet<Sprite>[m_Records.Count];

        for (int i = 0; i < sets.Length; i++)
        {
            sets[i] = new HashSet<Sprite>();

            foreach (var dic in m_Records[i])
            {
                sets[i].Add(dic.Key);
            }
        }

        foreach (Sprite sprite in sets[2]) // Use the middle column as the reference
        {
            // Check left side
            int leftStrike = 1;
            int rightStrike = 1;

            // Check left side
            if (sets[1].Contains(sprite))
            {
                leftStrike++;

                if (sets[0].Contains(sprite))
                {
                    leftStrike++;
                }
            }

            // Check right side
            if (sets[3].Contains(sprite))
            {
                rightStrike++;

                if (sets[4].Contains(sprite))
                {
                    rightStrike++;
                }
            }

            // Check if we have at least 3 consecutive identical symbols
            if (leftStrike + rightStrike - 1 >= 3)
            {
                Debug.Log($"Win: Found a series of {leftStrike + rightStrike - 1} consecutive {sprite.name} symbols.");
                CalculateScore();
            }
        }
    }

    private void CalculateScore()
    {
        Debug.Log("Score calculated based on the win condition.\n");
    }
}
