using Godot;

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

    public static class ColorForParticleBySealType
    {
        public static Color GetColor(this SealType type) => type switch
        {
            SealType.yellow => new Color("#D4C569"),
            SealType.red    => new Color("#FE968C"),
            SealType.green  => new Color("#55BA64"),
            _               => new Color("#418DA2")
        };
    }
}
