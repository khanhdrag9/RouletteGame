using UnityEngine;

namespace Game.Helper
{
    public static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if(component == null)
                component = gameObject.AddComponent<T>();
            return component;
        }

        public static Color StringToColor(string str)
        {
            if(string.IsNullOrEmpty(str))
                return new Color(0, 0, 0, 0);

            var p = str.Split(',');
            return new Color(float.Parse(p[0]), float.Parse(p[1]), float.Parse(p[2]), float.Parse(p[3]));
        }

        public static string ColorToString(Color color)
        {
            return $"{color.r},{color.g},{color.b},{color.a}";
        }

        public static T StringToEnum<T>(string value)
        {
            return (T) System.Enum.Parse(typeof(T), value, true);
        }

        public static string IntArrayToString(int[] array)
        {
            return string.Join(",", array);
        }

        public static int[] StringToIntArray(string str)
        {
            var p = str.Split(',');
            var result = new int[p.Length];
            for(int i = 0; i < result.Length; i++)
                result[i] = int.Parse(p[i]);

            return result; 
        }

        /// http://gizma.com/easing/
        public static float EaseInCubic(float time, float start, float end, float duration)
        {
            time /= duration;
            return (end - start) * time * time * time + start;
        }

        /// http://gizma.com/easing/
        public static float EaseOutCubic(float time, float start, float end, float duration)
        {
            time /= duration;
            time--;
            return (end - start)*(time*time*time + 1) + start;
        }
    }

    public static class AssetConverter
    {
        public static string SpriteToString(Sprite sprite)
        {
            return sprite.name;
        }
    }
}