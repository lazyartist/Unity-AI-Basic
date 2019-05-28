using UnityEngine;
using UnityEditor;

using System.Text;

public static class U
{
    public static StringBuilder d(params object[] args)
    {
        StringBuilder sd = new StringBuilder();
        for (int i = 0; i < args.Length; i++)
        {
            if (i != 0) sd.Append(", ");

            sd.Append(args[i]);
        }

        Debug.Log(sd.ToString());
        return sd;
    }

    public static StringBuilder f(params object[] args)
    {
        StringBuilder sd = new StringBuilder();
        for (int i = 0; i < args.Length; i++)
        {
            if (i != 0) sd.Append(", ");

            sd.Append(((float)args[i]).ToString("#.###"));
            //sd.Append(args[i]);
        }

        Debug.Log(sd.ToString());
        return sd;
    }
}