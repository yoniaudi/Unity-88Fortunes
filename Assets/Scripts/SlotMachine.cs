using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public Image[] Reels;
    public Sprite[] Symbols;
    public Button SpinButton;

    void Start()
    {
        SpinButton.onClick.AddListener(spinReels);
    }

    private void spinReels()
    {
        foreach (Image reel in Reels)
        {
            int randomIndex = Random.Range(0, Symbols.Length);

            reel.sprite = Symbols[randomIndex];
        }
    }
}
