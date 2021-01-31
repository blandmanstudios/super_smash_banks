using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDPanel : MonoBehaviour
{
    public Image panelImage;
    public Image leftPanelImage;
    public TextMeshProUGUI leftPanelTMPro;
    public Image upperCenterPanelImage;
    public TextMeshProUGUI upperCenterPanelTMPro;
    public Image lowerCenterPanelImage;
    public Image trendlineImage;
    public Image rightPanelImage;
    public TextMeshProUGUI rightPanelTMPro;

    Color colorHudText = new Color32(0,255,0,255);
    Color colorHudBackground = new Color32(0,0,0,255);
    Color colorInvisible = new Color32(255,255,255,0);

    // Start is called before the first frame update
    void Start()
    {
        SetHud();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHud() {
        panelImage.color = colorHudBackground;
        leftPanelTMPro.color = upperCenterPanelTMPro.color = rightPanelTMPro.color = colorHudText;
        leftPanelImage.color = upperCenterPanelImage.color = lowerCenterPanelImage.color = rightPanelImage.color = colorInvisible;
    }

    public void UpdateStockPriceDisplay() {
        var stockPriceFiniteDecimalPlaces = Runner_GameScene.StockPrice.ToString("F2");
        upperCenterPanelTMPro.text = $"Stock Price\n{stockPriceFiniteDecimalPlaces}";
    }

    public void UpdatePlayerStatsDisplay(Battler battler) {
        // Assumption: Only the player's battler will be passed in.
        var netWorthFiniteDecimalPlaces = battler.netWorth.ToString("F0");
        var stockFiniteDecimalPlaces = battler.Stock.ToString("F2");
        leftPanelTMPro.text = $"Net Worth($)\n{netWorthFiniteDecimalPlaces}\nShares\n{stockFiniteDecimalPlaces}";
    }
}
