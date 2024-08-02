using TMPro;
using UnityEngine;

public class CoinTextController : MonoBehaviour
{
    private TextMeshProUGUI coinAmountTxt;
    public TextMeshProUGUI CoinAmountText
    {
        get
        {
            if(coinAmountTxt == null)
            coinAmountTxt = GetComponent<TextMeshProUGUI>();

            return coinAmountTxt;
        }
    }

    private void OnEnable()
    {
        PlayerSimulationController.OnItemSell += UpdateCoinAmountText;
    }
    private void OnDisable()
    {
        PlayerSimulationController.OnItemSell -= UpdateCoinAmountText;
    }

    private void UpdateCoinAmountText(float amount)
    {
        CoinAmountText.text = amount.ToString();
    }


}
