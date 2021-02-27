using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CsvLoader
{
    public static List<List<string>> Load(string path)
    {
        List<List<string>> ret = new List<List<string>>();
        var csv = Resources.Load<TextAsset>(path);
        using (StringReader sr = new StringReader(csv.text))
        {
            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                var splits = line.Split(',');
                ret.Add(splits.ToList());
            }
        }

        return ret;
    }
}
