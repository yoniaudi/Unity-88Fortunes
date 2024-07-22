using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    //private List<Image[]> m_Reels;
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
        for (int i = 0; i < m_Reels[0].Length; i++)
        {
            for (int j = 0; j < m_Reels.Length; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, Symbols.Length);

                m_Reels[j][i].sprite = Symbols[randomIndex];
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

        //checkWinCondition();
    }
    /*
    private void checkWinCondition()
    {
        for (int i = 1; i < Reels.Length; i++)
        {
            if (Reels[i - 1].sprite != Reels[i].sprite)
            {
                Console.WriteLine("Try Again!");
                break;
            }
        }

        Debug.Log("You Win!");
    }*/
}
