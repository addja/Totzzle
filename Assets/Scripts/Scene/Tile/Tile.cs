using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public Animator animator;
    public TextMeshProUGUI displayText;

    // TODO: this screams of polimorphism. For the moment just hacking this here
    public enum TileType
    {
        countdown,
        origin,
        trigger,
    }
    public TileType type;
    public uint counterInitialValue;

    Dictionary<TileType, int> typeAnimatorDic = new Dictionary<TileType, int>{
        {TileType.origin, -1},
        {TileType.trigger, -2},
        {TileType.countdown, 1},
    };
    bool countDownOn = false;
    uint counter = 0;

    public void StartCountdown()
    {
        countDownOn = true;
    }

    private void Start()
    {
        AnimateTile(typeAnimatorDic[type]);

        if (type == TileType.countdown)
        {
            counter = counterInitialValue;
            UpdateDisplayedValue(counter.ToString());
        }
    }


    public void UpdateTile()
    {
        if (countDownOn && counter != 0)
        {
            counter--;
            if (counter == 0)
            {
                Destroy(gameObject);
            }
            UpdateDisplayedValue(counter.ToString());
        }
    }

    void AnimateTile(int typeAnimator)
    {
        animator.SetInteger("tileTypeAnimator", typeAnimator);
    }

    void UpdateDisplayedValue(string value)
    {
        displayText.text = value;
    }
}
