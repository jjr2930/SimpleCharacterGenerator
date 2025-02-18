using System.Collections.Generic;
using UnityEngine;

public class PatternFileListViewer : MonoBehaviour
{
    [SerializeField] List<TextAsset> patternFiles = new List<TextAsset>(15);
    [SerializeField] Transform content;
    [SerializeField] PatternListItem patternPrefab;
    // Update is called once per frame

    private void Start()
    {
        for (int i = 0; i < patternFiles.Count; i++)
        {
            var newItem = Instantiate(patternPrefab, content);
            newItem.SetPatternFile(patternFiles[i], i.ToString());
        }

        patternPrefab.gameObject.SetActive(false);
    }
}
