using Vector4 = System.Numerics.Vector4;
using System.Numerics;

namespace MyGame
{
    // the values 
    public struct Values
    {
        public static float ForceLightScene = 20f;

        // bloom
        public static bool isRenderBloom = true; 
        public static float new_bloom_exp = 0.557f;
        public static float new_bloom_streng = 0.213f;
        public static float new_bloom_gama = 0.425f;
        public static float filterRadius = 0f;
        public static float new_bloom_filmGrain = -0.1f;
        public static float nitidezStrengh = 0f;
        public static int vibrance = 10;
        public static bool activeNegative = false;


    }
}