using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelGenerator : MonoBehaviour
{
    [SerializeField] ShapeSetter maleInstance = null;
    [SerializeField] ShapeSetter femaleInstance = null;
    [SerializeField] ShapeSetter neutralInstance = null;

    public void Awake()
    {
        //deactivate male, female, neutral
        maleInstance.gameObject.SetActive(false);
        femaleInstance.gameObject.SetActive(false);
        neutralInstance.gameObject.SetActive(false);
    }
}
