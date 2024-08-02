using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HelperController : MonoBehaviourSingletonPersistent<HelperController>
{
    [SerializeField] List<PanelData> data;

    [System.Serializable]
    class PanelData
    {
        public string name;
        public GameObject gameObject;
        public HelpType helpType;
    }

    private void Start()
    {
        foreach (var item in data)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ShowHelper(HelpType type)
    {
        foreach (var item in data)
        {
            item.gameObject.SetActive(false);
        }
        foreach (var item in data)
        {
            if (item.helpType == type)
            {
                item.gameObject.SetActive(true);
                break;
            }
        }
    }
}

public enum HelpType
{
    FindLiveStock,
    GoGrazePanel,
    TakeAnimalGrazePanel,
    REstPanel,  
    DefendPanel,
    CollectPanel,
    ReturnPanel,
    BarnPanel,  
    FindClient,
    BuyPanel
}
