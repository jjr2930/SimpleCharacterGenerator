using UnityEngine;

public class PatternReaderTester : MonoBehaviour
{
    [SerializeField]
    TextAsset jsonFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Read the pattern from the file
        PatternDataStructures pattern = PatternReader.ReadPatternWithJson(jsonFile.text);
        
        // PatternDataStructures pattern = new PatternDataStructures();
        // var newPattern = new PatternDataStructures.Pattern();
        
        // newPattern.panels = new System.Collections.Generic.Dictionary<string, PatternDataStructures.Pattern.Panel>();
        // var newPanel = new PatternDataStructures.Pattern.Panel();
        
        // var vertices = new System.Collections.Generic.List<System.Collections.Generic.List<double>>();
        // vertices.Add(new System.Collections.Generic.List<double>(){1.0, 2.0, 3.0});
        // vertices.Add(new System.Collections.Generic.List<double>(){4.0, 5.0, 6.0});
        // vertices.Add(new System.Collections.Generic.List<double>(){7.0, 8.0, 9.0});

        // newPanel.vertices = vertices;        

        // newPattern.panels.Add("one", newPanel);

        // pattern.pattern = newPattern;

        // var json = JsonConvert.SerializeObject(pattern);

        // Debug.Log("Pattern: " + json);
    }
}
