using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Animator animator;

    // TODO: this screams of polimorphism. For the moment just hacking this here
    enum TileType
    {
        countdown,
        origin,
        trigger,
        setter,
    }
    Dictionary<TileType, int> tileTypeAnimatorDic = new Dictionary<TileType, int>{
        {TileType.origin, -1},
        {TileType.trigger, -2},
        {TileType.setter, -3},
    };
    bool countDownNextTurn = false;
    bool countDownOn = false;
    uint counter = 0;
    TileType tileType;

    public void StartCountdown()
    {
        countDownNextTurn = true;
    }

    public void SetTile(char type)
    {
        switch (type)
        {
            case 'a':
                tileType = TileType.origin;
                break;
            case 'b':
                tileType = TileType.trigger;
                break;
            case 'c':
                tileType = TileType.setter;
                break;
            default:
                tileType = TileType.countdown;
                counter = (uint)type - '0';
                break;
        }

        if (counter != 0)
        {
            AnimateTile((int)counter);
        }
        else
        {
            AnimateTile(tileTypeAnimatorDic[tileType]);
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

    void AnimateTile(int tileTypeAnimator)
    {
        animator.SetInteger("tileTypeAnimator", tileTypeAnimator);
    }

    public bool BadTile()
    {
        return counter == 0 && tileType == TileType.countdown;
    }
}
