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
    }
}
