using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedSkinGridScript : MonoBehaviour
{
    public GameObject Skin;
    public GameObject SkinCard;
    public Transform gridParent;
    public List<SkinData> skins;
    public GridLayoutGroup gridLayoutGroup;

    private List<SkinData> LockedSkins;

    private int cellSize;

    public Material grayscaleMat;

    // Start is called before the first frame update
    void Start()
    {
        grayscaleMat.SetFloat("_GrayscaleAmount", 1f);

        GetLockedSkins();

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

    private void GetLockedSkins()
    {
        LockedSkins = new List<SkinData>();
        if (PlayerPrefs.GetString("Default Skin Status") == "")
        {
            PlayerPrefs.SetString("Default Skin Status", "On");

            for (int i = 1; i < skins.Count; ++i)
            {
                PlayerPrefs.SetString($"{skins[i].sprite.name} Status", "Off");
            }
        }

        for (int i = 0; i < skins.Count; ++i)
        {
            if (PlayerPrefs.GetString($"{skins[i].sprite.name} Status") == "Off")
            {
                LockedSkins.Add(skins[i]);
            }
        }
    }

    public void PopulateGrid()
    {
        foreach (SkinData skin in LockedSkins)
        {
            AddSkin(skin);
        }
    }

    public void RemoveSkin(string skinName)
    {
        foreach (Transform skin in gridParent)
        {
            if(skin.Find("Image").GetComponent<Image>().sprite.name == skinName)
            {
                Destroy(skin.gameObject);
                break;
            }
        }
    }
        
    private void AddSkin(SkinData skin)
    {

        GameObject cell = Instantiate(Skin, gridParent);

        GameObject cellImageChild = cell.transform.Find("Image").gameObject;
        GameObject Bg = cell.transform.Find("BG").gameObject;


        Button buttonComponent = cell.GetComponent<Button>();
        Image imageComponent = cellImageChild.GetComponent<Image>();

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

        buttonComponent.onClick.AddListener(
            () =>
            {
                GameObject skinData = Instantiate(SkinCard);

                skinData.GetComponent<SkinDataScript>().skinData = skin;
            }
        );
    }
}
