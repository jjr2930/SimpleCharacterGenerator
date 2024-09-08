using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class ModelGenerator : MonoBehaviour
{
    [SerializeField] ShapeSetter maleInstance = null;
    [SerializeField] ShapeSetter femaleInstance = null;
    [SerializeField] ShapeSetter neutralInstance = null;

    ShapeSetter selectedShape = null;

    public ShapeSetter SelectedShape { get => selectedShape; }

    public void Awake()
    {
        //deactivate male, female, neutral
        maleInstance.gameObject.SetActive(false);
        femaleInstance.gameObject.SetActive(false);
        neutralInstance.gameObject.SetActive(false);
    }

    public void SetGender(string gender)
    {
        var all = new ShapeSetter[] { maleInstance, femaleInstance, neutralInstance };
        switch (gender)
        {
            case "male": ActiveModel(maleInstance, all); break;
            case "female": ActiveModel(femaleInstance, all); break;
            case "neutral": ActiveModel(neutralInstance, all); break;

            default:
                Debug.LogError($"{gender} not supported");
                break;
        }
    }

    public void SetShape(float[] shapes)
    {
        Assert.AreEqual(shapes.Length , 
            ShapeSetter.SHAPE_PROPERTY_COUNT, $"{shapes.Length} != {ShapeSetter.SHAPE_PROPERTY_COUNT}");

        selectedShape.SetShape(shapes);
    }

    public void SetTexture(string texturePath)
    {
        Assert.IsFalse(string.IsNullOrEmpty(texturePath));
        Assert.IsTrue(File.Exists(texturePath), $"there is no texture at {texturePath}");

        selectedShape.SetTexture(texturePath);
    }


    void ActiveModel(ShapeSetter activatedModel, params ShapeSetter[] array)
    {        
        foreach (var item in array)
        {
            item.gameObject.SetActive(item == activatedModel);
        }
        
        selectedShape = activatedModel;
    }
}
