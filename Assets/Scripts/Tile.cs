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
    bool countDownOn = false;
    uint counter = 0;
    TileType tileType;

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

        int tileTypeAnimator = 0;
        if (counter != 0)
        {
            tileTypeAnimator = (int)counter;
        }
        else
        {
            tileTypeAnimator = tileTypeAnimatorDic[tileType];
        }
        animator.SetInteger("tileTypeAnimator", tileTypeAnimator);
    }
}
