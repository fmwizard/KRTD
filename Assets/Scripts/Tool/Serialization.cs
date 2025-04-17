using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public static class Serialization
{
    public static string SaveBoardToJson(BoardData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        json = System.Text.RegularExpressions.Regex
            .Replace(json,
                    @"\[\s*(?:(?:""[^""]*""|\d+)\s*,\s*)*(?:""[^""]*""|\d+)\s*\]",
                    m => m.Value
                            .Replace("\r\n", "")
                            .Replace("\n", "")
                            .Replace("  ", "")
            );
        return json;
    }

    public static void SaveJsonToFile(string json, string filePath)
    {
        System.IO.File.WriteAllText(filePath, json);
    }
}
