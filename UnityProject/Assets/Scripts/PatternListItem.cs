using UnityEngine;

public class PatternListItem : MonoBehaviour
{
    [SerializeField] TextAsset patternFile;
    [SerializeField] TMPro.TextMeshProUGUI patternName;
    [SerializeField] PatternGeneratorTester patternGeneratorTester;
    public void SetPatternFile(TextAsset patternFile, string name)
    {
        patternName.text = name;
        this.patternFile = patternFile;
    }

    public void OnClicked()
    {
        patternGeneratorTester.OnClicked(patternFile);
    }
}
