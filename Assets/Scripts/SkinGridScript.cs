using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinGridScript : MonoBehaviour
{
    public GameObject Skin;
    public Transform gridParent;
    public List<Sprite> sprites;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject ListScreen;
    public GameObject GameScreen;
    public GameObject Square;
    
    private GameObject currentSelectedCell = null;
    private int cellSize;

    void Start()
    {
        ListScreen.SetActive(false);

        int size;
        if(Screen.width > Screen.height)
        {
            size = Screen.height;
        } else
        {
            size = Screen.width;
        }
        cellSize = (size - 40) / 4 - 20;
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        PopulateGrid(sprites);
    }

    public void PopulateGrid(List<Sprite> spriteList)
    {
        SpriteRenderer squareSpriteRenderer = Square.GetComponent<SpriteRenderer>();

        foreach (Sprite sprite in spriteList)
        {
            GameObject cell = Instantiate(Skin, gridParent);
            string spriteName = PlayerPrefs.GetString("Skin");
            if (spriteName == "")
            {
                spriteName = "Default Skin";
            }
            if(sprite.name == spriteName)
                currentSelectedCell = cell;

            Image imageComponent = cell.GetComponent<Image>();
            Button buttonComponent = cell.GetComponent<Button>();

            if (imageComponent != null)
                imageComponent.sprite = sprite;


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

        //set 'selected' sign
        currentSelectedCell.transform.Find("SelectedSign").gameObject.SetActive(true);
    }

    public void Close()
    {
        ListScreen.SetActive(false);
        GameScreen.SetActive(true);
        Square.SetActive(true);
    }

    public void Open() {
        ListScreen.SetActive(true);
        GameScreen.SetActive(false);
        Square.SetActive(false);
    }
}
