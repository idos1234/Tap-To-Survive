using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Recorder.OutputPath;
using static UnityEngine.UI.Image;

public class SkinDataScript : MonoBehaviour
{
    public SkinData skinData;
    public GameObject card;
    private UnlockedSkinGridScript unlockedSkinGridScript;
    private LockedSkinGridScript lockedSkinGridScript;
    private Text coins;

    void Start()
    {
        //set values
        coins = FindInactiveObjectByTag("HomeCoin").GetComponent<Text>();
        unlockedSkinGridScript = GameObject.FindGameObjectWithTag("Unlocked Grid").GetComponent<UnlockedSkinGridScript>();
        lockedSkinGridScript = GameObject.FindGameObjectWithTag("Locked Grid").GetComponent<LockedSkinGridScript>();

        Text skinName = card.transform.Find("Skin - name").GetComponent<Text>();
        GameObject skinImage = card.transform.Find("Skin - image").gameObject;

        Transform button = card.transform.Find("Buy Skin");
        Text skinPrice = button.Find("Price").GetComponent<Text>();
        GameObject coin = button.Find("Coin").gameObject;

        //set sizes
        skinName.fontSize = Mathf.RoundToInt(Screen.width * 0.075f);
        skinImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3, Screen.width / 3);
        coin.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 15, Screen.width / 15);
        skinPrice.fontSize = Mathf.RoundToInt(Screen.width * 0.05f);
        card.transform.Find("Top").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 2.75f, card.transform.Find("Top").GetComponent<RectTransform>().sizeDelta.y);
        card.transform.Find("Top").Find("Close").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 15, Screen.width / 15);

        card.GetComponent<VerticalLayoutGroup>().padding.left = Screen.width / 100;
        card.GetComponent<VerticalLayoutGroup>().padding.right = Screen.width / 100;
        card.GetComponent<VerticalLayoutGroup>().padding.top = Screen.height / 50;
        card.GetComponent<VerticalLayoutGroup>().padding.bottom = Screen.height / 50;

        card.GetComponent<Outline>().effectDistance = new Vector2(Screen.width / 50, Screen.width / 50);

        //set data
        skinName.text = skinData.sprite.name;
        skinImage.GetComponent<Image>().sprite = skinData.sprite;
        skinPrice.text = skinData.price.ToString();

        //set color
        switch (skinData.rarity)
        {
            case Rarity.Common: // Common
                card.GetComponent<Outline>().effectColor = new Color32(160, 160, 160, 255);
                break;
            case Rarity.Rare: // Rare
                card.GetComponent<Outline>().effectColor = new Color32(0, 200, 60, 255);
                break;
            case Rarity.SuperRare: // Super Rare
                card.GetComponent<Outline>().effectColor = new Color32(0, 220, 255, 255);
                break;
            case Rarity.Epic: // Epic
                card.GetComponent<Outline>().effectColor = new Color32(255, 0, 160, 255);
                break;
            case Rarity.Legendary: // Legendary
                card.GetComponent<Outline>().effectColor = new Color32(255, 200, 50, 255);
                break;
            default:
                card.GetComponent<Outline>().effectColor = new Color32(0, 0, 0, 0);
                break;
        }

        int playerCurrentCoins = PlayerPrefs.GetInt("Coins");
        if(playerCurrentCoins < skinData.price)
        {
            button.GetComponent<Image>().color = new Color32(255, 177, 177, 225);
        }

        button.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if(playerCurrentCoins < skinData.price) { Handheld.Vibrate(); }
            else
            {
                PlayerPrefs.SetInt("Coins", playerCurrentCoins - skinData.price);
                coins.text = (playerCurrentCoins - skinData.price).ToString();

                PlayerPrefs.SetString($"{skinData.sprite.name} Status", "On");

                unlockedSkinGridScript.AddToGrid(skinData);
                lockedSkinGridScript.RemoveSkin(skinData.sprite.name);

                Close();
            }
        });


    }

    public void Close()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    GameObject FindInactiveObjectByTag(string tag)
    {
        GameObject[] all = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in all)
        {
            if (go.CompareTag(tag) && !go.activeInHierarchy && go.hideFlags == HideFlags.None)
            {
                return go;
            }
        }
        return null;
    }
}
