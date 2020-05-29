using UnityEngine;

public static class HelperClass 
{
    public static Color GetColor(int _index)
    {
        switch (_index)
        {
            case 0:
                return Color.white;

            case 1:
                return new Color(0, 0.4f, 1);

            case 2:
                return new Color(1, 0.1f, 0.2f);




            case 100:
                return Color.yellow;
        }

        return Color.grey;
    }
}
