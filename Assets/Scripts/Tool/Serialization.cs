using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class Serialization
{

    public static string saveFileName = "editor_level.json";

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

    public static BoardData LoadBoardFromFile()
    {
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, saveFileName);
        string json;
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"找不到文件: {fullPath}");
            return null;
        }
        json = File.ReadAllText(fullPath);

        BoardData data = JsonConvert.DeserializeObject<BoardData>(json);
        if (data == null)
        {
            Debug.LogError("反序列化 JSON 失败");
            return null;
        }
        return data;
    }
}
