using System;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;

public class UIPanelController : MonoBehaviour
{
    #region Self Varibles

    #region Serialized Variables

    [SerializeField] private List<Transform> layers = new List<Transform>();

    #endregion

    #endregion

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        
    }

    private void UnSubscribeEvents()
    {
        
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    
    [Button("OnOpenPanel")]

    private void OnOpenPanel(UIPanelTypes type, int layerValue)
    {
        Instantiate(Resources.Load<GameObject>($"Screen/{type}Panel"), layers[layerValue]);
    }
    
    [Button("OnClosePanel")]
    
    private void OnClosePanel(int layerValue)
    {
        if (layers[layerValue].childCount > 0)
        {
            Destroy(layers[layerValue].GetChild(0).gameObject);
        }
    }

    private void OnCloseAllPanels()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].childCount > 0)
            {
                Destroy(layers[i].GetChild(0).gameObject);
            }
        }
    }
}
