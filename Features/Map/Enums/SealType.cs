using Godot;
using System;

namespace TheDragonsPuzzleSeals.Features.Map
{
    public enum SealType
    {
        blue,
        red,
        green,
        yellow,
        match_4_H,
        match_4_V,
        match_5,
        match_TL,
    }

    public enum SealAction
    {
        Swap,
        Fall,
        Explosion,
        Refill,
    }
}
