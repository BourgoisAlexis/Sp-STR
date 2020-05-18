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
                return Color.blue;

            case 2:
                return Color.red;




            case 100:
                return Color.yellow;
        }

        return Color.grey;
    }
}
