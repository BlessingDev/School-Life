﻿using UnityEngine;
using System.Collections.Generic;

public class ShopManager : Manager<ShopManager>
{
    [SerializeField]
    private GameObject preShopPopup = null;
    private GameObject shopPopup = null;
    public GameObject ShopPopup
    {
        get
        {
            return shopPopup;
        }
    }

    private Dictionary<SkinType, GameObject[]> shoppingList;
    private ScaleEffecter[] effecters = null;

    private SkinType shopType = 0;       // 어떤 종류의 제품을 구입할건지
    public SkinType ShopType
    {
        get
        {
            return shopType;
        }
        set
        {
            shopType = value;
        }
    }

    private string selectedSkinName;
    public string SelectedSkinName
    {
        get
        {
            return selectedSkinName;
        }
        set
        {
            RefreshExplanation();
            RefreshIcon();
            selectedSkinName = value;
        }
    }

    public override void Init()
    {
        Start();
        base.Init();
    }

    // Use this for initialization
    void Start ()
    {
        if(!IsInited)
        {
            shoppingList = new Dictionary<SkinType, GameObject[]>();

            for (int i = 1; i <= 5; i += 1)
            {
                var objs = Resources.LoadAll<GameObject>("Prefabs/Shop/" + ((SkinType)i).ToString() + "/");
                shoppingList.Add((SkinType)i, objs);
            }

            effecters = FindObjectsOfType<ScaleEffecter>();

            if (preShopPopup == null)
            {
                Debug.LogWarning("The Prefab NOT PREPARED");
            }
        }
    }

    public void OpenShopPopup(SkinType shopType)
    {
        this.shopType = shopType;

        UIManager.Instance.SetEnableTouchLayer("Main", true);
        for(int i = 0; i < effecters.Length; i += 1)
        {
            effecters[i].enabled = false;
        }

        shopPopup = Instantiate(preShopPopup);
        shopPopup.transform.parent = UIManager.Instance.Canvas.transform;
        shopPopup.transform.localPosition = Vector3.zero;
        shopPopup.transform.localScale = Vector3.one;

        GameObject[] objs;
        if(shoppingList.TryGetValue(shopType, out objs))
        {
            RectTransform trans = shopPopup.GetComponent<ShopPopup>().ScrolledRect.GetComponent<RectTransform>();
            int width = objs.Length * 150 + (objs.Length - 1) * 50;
            Vector2 offset = trans.offsetMax;
            offset.x = trans.offsetMin.x + width + 20;
            trans.offsetMax = offset;

            for (int i = 0; i < objs.Length; i += 1)
            {
                GameObject obj = Instantiate(objs[i]);
                obj.transform.SetParent(trans.transform);
                obj.transform.localScale = Vector3.one;
            }
        }
    }

    public void ClosePopup()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        for (int i = 0; i < effecters.Length; i += 1)
        {
            effecters[i].enabled = true;
        }

        Destroy(shopPopup);
        shopPopup = null;
    }

    public void RefreshExplanation()
    {

    }

    public void RefreshIcon()
    {

    }

    public void Use()
    {
        if(shopType < SkinType.Costume)
        {
            GameManager.Instance.SetSkinName(shopType, selectedSkinName);
        }
        else
        {
            GameManager.Instance.CostumeCode = int.Parse(selectedSkinName);
        }
    }
}
