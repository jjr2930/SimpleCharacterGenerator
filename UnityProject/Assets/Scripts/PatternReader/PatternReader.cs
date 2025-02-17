using Newtonsoft.Json;
using UnityEngine;

public static class PatternReader 
{
    public static PatternDataStructures ReadPatternWithPath(string path)
    {
        string json = System.IO.File.ReadAllText(path);

        return ReadPatternWithJson(json);
    }

    public static PatternDataStructures ReadPatternWithJson(string json)
    {
        return JsonConvert.DeserializeObject<PatternDataStructures>(json);
    }
}
