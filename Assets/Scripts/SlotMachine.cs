using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    private Dictionary<int, Dictionary<Sprite, int>> m_Records = null;
    private Image[][] m_Reels = null;
    private StringBuilder m_ScoreStr = null;
    private int m_TotalScore = 0;
    public TextMeshProUGUI Score = null;
    public Image[] Reels1 = null;
    public Image[] Reels2 = null;
    public Image[] Reels3 = null;
    public Image[] Reels4 = null;
    public Image[] Reels5 = null;
    public Sprite[] Symbols = null;
    public Button SpinButton = null;

    void Start()
    {
        m_Records = new Dictionary<int, Dictionary<Sprite, int>>();
        m_Reels = new Image[3][];
        m_ScoreStr = new StringBuilder();

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
                bool isWildSpritePositionLegal = true;

                m_Reels[j][i].sprite = Symbols[randomIndex];
                isWildSpritePositionLegal = this.isWildSpritePositionLegal(m_Reels[j][i].sprite, i);

                while (isWildSpritePositionLegal == false)
                {
                    randomIndex = UnityEngine.Random.Range(0, Symbols.Length);
                    m_Reels[j][i].sprite = Symbols[randomIndex];
                    isWildSpritePositionLegal = this.isWildSpritePositionLegal(m_Reels[j][i].sprite, i);
                }

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

        checkWinCondition();
    }

    private bool isWildSpritePositionLegal(Sprite i_Sprite, int i_Index)
    {
        bool isLegal = true;

        if (i_Sprite.name == "icon_3" && (i_Index == 0 || i_Index == 4))
        {
            isLegal = false;
        }

        return isLegal;
    }

    private void checkWinCondition()
    {
        HashSet<Sprite>[] sets = new HashSet<Sprite>[m_Records.Count];

        Score.text = "Score: ";

        for (int i = 0; i < sets.Length; i++)
        {
            sets[i] = new HashSet<Sprite>();

            foreach (var dic in m_Records[i])
            {
                sets[i].Add(dic.Key);
            }
        }

        foreach (Sprite sprite in sets[2])
        {
            int leftStrike = 1;
            int rightStrike = 1;

            if (sets[1].Contains(sprite) || sets[1].Any(sprite => sprite.name == "icon_3"))
            {
                leftStrike++;

                if (sets[0].Contains(sprite))
                {
                    leftStrike++;
                }
            }

            if (sets[3].Contains(sprite) || sets[3].Any(sprite => sprite.name == "icon_3"))
            {
                rightStrike++;

                if (sets[4].Contains(sprite))
                {
                    rightStrike++;
                }
            }

            if (leftStrike + rightStrike - 1 >= 3)
            {
                Debug.Log($"Win: Found a series of {leftStrike + rightStrike - 1} consecutive {sprite.name} symbols.");
                calculateScore(sprite);
            }
        }

        Score.text = Score.text.TrimEnd(' ', '+');
        Score.text += $" = {m_TotalScore}";
    }

    private void calculateScore(Sprite i_Sprite)
    {
        int score = 1;

        foreach (var dic in m_Records)
        {
            if (dic.Value.ContainsKey(i_Sprite))
            {
                m_ScoreStr.Append(dic.Value[i_Sprite]);
                score *= dic.Value[i_Sprite];
                m_ScoreStr.Append(" * ");
            }

        }

        m_TotalScore += score;
        Score.text += $"{m_ScoreStr.ToString().TrimEnd(' ', '*')} + ";
        Debug.Log($"Score: {score}.{Environment.NewLine}");
    }
}
