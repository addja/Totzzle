using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Animator animator;

    // TODO: this screams of polimorphism. For the moment just hacking this here
    public enum TileType
    {
        countdown,
        origin,
        trigger,
        setter,
    }
    public TileType type;

    Dictionary<TileType, int> typeAnimatorDic = new Dictionary<TileType, int>{
        {TileType.origin, -1},
        {TileType.trigger, -2},
        {TileType.setter, -3},
        {TileType.countdown, 1},
    };
    bool countDownNextTurn = false;
    bool countDownOn = false;
    uint counter = 0;

    public void StartCountdown()
    {
        countDownNextTurn = true;
    }

    private void Start() {
        AnimateTile(typeAnimatorDic[type]);

        if (type == TileType.countdown)
        {
            // TODO change tiles to use canvas for numbering
            counter = 1;
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
            AnimateTile((int)counter);
        }

        // TODO: Don't like this, need to think something elegant
        countDownOn = countDownNextTurn;
    }

    void AnimateTile(int typeAnimator)
    {
        animator.SetInteger("tileTypeAnimator", typeAnimator);
    }

    public bool BadTile()
    {
        return counter == 0 && type == TileType.countdown;
    }
}
