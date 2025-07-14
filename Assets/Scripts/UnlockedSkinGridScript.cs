using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedSkinGridScript : MonoBehaviour
{
    public GameObject Skin;
    public Transform gridParent;
    public List<SkinData> skins;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject ListScreen;
    public GameObject GameScreen;
    public GameObject Square;

    private List<SkinData> UnlockedSkins;

    private GameObject currentSelectedCell = null;
    private int cellSize;

    private SpriteRenderer squareSpriteRenderer;

    void Start()
    {
        squareSpriteRenderer = Square.GetComponent<SpriteRenderer>();

        ListScreen.SetActive(false);
        GetUnlockedSkins();

        int size;
        if (Screen.width > Screen.height)
        {
            size = Screen.height;
        }
        else
        {
            size = Screen.width;
        }
        cellSize = (size - 40) / 4 - 20;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        PopulateGrid();
    }

    private void GetUnlockedSkins()
    {
        UnlockedSkins = new List<SkinData> ();
        if(PlayerPrefs.GetString("Default Skin Status") == "")
        {
            PlayerPrefs.SetString("Default Skin Status", "On");

            for (int i = 1; i < skins.Count; ++i)
            {
                PlayerPrefs.SetString($"{skins[i].sprite.name} Status", "Off");
            }
        }

        for (int i = 0; i < skins.Count; ++i)
        {
            if (PlayerPrefs.GetString($"{skins[i].sprite.name} Status") == "On")
            {
                UnlockedSkins.Add(skins[i]);
            }
        }
    }

    public void PopulateGrid()
    {
        foreach (SkinData skin in UnlockedSkins)
        {

            AddToGrid(skin);
            
        }

        //set 'selected' sign
        currentSelectedCell.transform.Find("SelectedSign").gameObject.SetActive(true);
    }

    public void AddToGrid(SkinData skin)
    {
        GameObject cell = Instantiate(Skin, gridParent);

        string spriteName = PlayerPrefs.GetString("Skin");
        if (spriteName == "")
        {
            spriteName = "Default Skin";
        }
        if (skin.sprite.name == spriteName)
            currentSelectedCell = cell;

        GameObject cellImageChild = cell.transform.Find("Image").gameObject;
        GameObject Bg = cell.transform.Find("BG").gameObject;


        Image imageComponent = cellImageChild.GetComponent<Image>();
        Button buttonComponent = cell.GetComponent<Button>();

        if (imageComponent != null)
            imageComponent.sprite = skin.sprite;

        switch (skin.rarity)
        {
            case Rarity.Common: // Common
                Bg.GetComponent<Image>().color = new Color32(192, 192, 192, 100);
                break;
            case Rarity.Rare: // Rare
                Bg.GetComponent<Image>().color = new Color32(0, 189, 41, 100);
                break;
            case Rarity.SuperRare: // Super Rare
                Bg.GetComponent<Image>().color = new Color32(0, 195, 255, 100);
                break;
            case Rarity.Epic: // Epic
                Bg.GetComponent<Image>().color = new Color32(255, 45, 180, 100);
                break;
            case Rarity.Legendary: // Legendary
                Bg.GetComponent<Image>().color = new Color32(255, 180, 25, 150);
                break;
            default:
                Bg.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                break;
        }


        // Configure SelectedSign
        Transform signTransform = cell.transform.Find("SelectedSign");

        if (signTransform != null)
        {
            RectTransform signRect = signTransform.GetComponent<RectTransform>();
            if (signRect != null)
            {
                float signHeight = cellSize / 4f;
                gridLayoutGroup.spacing = new Vector2(10, signHeight);
                signRect.sizeDelta = new Vector2(signRect.sizeDelta.x, signHeight);
                signRect.anchoredPosition = new Vector2(0, 0); // Anchored to bottom center
            }

            Text text = signTransform.GetComponentInChildren<Text>();
            if (text != null)
            {
                text.fontSize = Mathf.RoundToInt(cellSize / 5f); // You can tweak the divisor for better fit
                text.rectTransform.offsetMin = new Vector2(10, 0);
            }

            signTransform.gameObject.SetActive(false); // Hide initially
        }

        buttonComponent.onClick.AddListener(() => {
            // Update main square
            squareSpriteRenderer.sprite = imageComponent.sprite;
            PlayerPrefs.SetString("Skin", squareSpriteRenderer.sprite.name);

            // Hide "Selected" on previous cell
            if (currentSelectedCell != null)
            {
                Transform oldSign = currentSelectedCell.transform.Find("SelectedSign");
                if (oldSign != null) oldSign.gameObject.SetActive(false);
            }

            // Show "Selected" on this cell
            Transform newSign = cell.transform.Find("SelectedSign");
            if (newSign != null) newSign.gameObject.SetActive(true);

            // Update current selected
            currentSelectedCell = cell;
        });
    }

    public void Close()
    {
        ListScreen.SetActive(false);
        GameScreen.SetActive(true);
        Square.SetActive(true);
    }

    public void Open()
    {
        ListScreen.SetActive(true);
        GameScreen.SetActive(false);
        Square.SetActive(false);
    }
}

public enum Rarity { Common, Rare, SuperRare, Epic, Legendary }

[System.Serializable]
public class SkinData
{
    public Sprite sprite;
    public Rarity rarity;
    public int price;
}
