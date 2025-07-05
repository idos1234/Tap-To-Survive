using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CountingAnimation : MonoBehaviour
{
    public Text targetText;
    public Text coinsEarned;
    public float duration;
    public Text SumCoins;

    public void StartCounting()
    {
        int from = PlayerPrefs.GetInt("Coins");
        targetText.text = from.ToString();
        int to = from + Int32.Parse(coinsEarned.text);
        coinsEarned.text = "0";
        PlayerPrefs.SetInt("Coins", to);
        SumCoins.text = to.ToString();


        StartCoroutine(CountAnimation(from, to, duration));
    }

    private IEnumerator CountAnimation(int startValue, int endValue, float duration)
    {
        yield return new WaitForSeconds(1f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            targetText.text = currentValue.ToString();
            yield return null;
        }

        // Ensure final value is exactly endValue
        targetText.text = endValue.ToString();
    }
}
