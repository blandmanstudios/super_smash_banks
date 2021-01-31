using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Image hudPanelImage;
    public Image hudLeftPanelImage;
    public TextMeshProUGUI hudLeftPanelTMPro;
    public Image hudUpperCenterPanelImage;
    public TextMeshProUGUI hudUpperCenterPanelTMPro;
    public Image hudLowerCenterPanelImage;
    public Image hudTrendlineImage;
    public Image hudRightPanelImage;
    public TextMeshProUGUI hudRightPanelTMPro;

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
        hudPanelImage.color = colorHudBackground;
        hudLeftPanelTMPro.color = hudUpperCenterPanelTMPro.color = hudRightPanelTMPro.color = colorHudText;
        hudLeftPanelImage.color = hudUpperCenterPanelImage.color = hudLowerCenterPanelImage.color = hudRightPanelImage.color = colorInvisible;
    }

    public void UpdateStockPriceDisplay() {
        var stockPriceFiniteDecimalPlaces = Runner_GameScene.StockPrice.ToString("F2");
        hudUpperCenterPanelTMPro.text = $"Stock Price\n{stockPriceFiniteDecimalPlaces}";
    }

    public void UpdatePlayerStatsDisplay(Battler battler) {
        // Assumption: Only the player's battler will be passed in.
        var netWorthFiniteDecimalPlaces = string.Format("{0:n0}", battler.netWorth);
        var stockFiniteDecimalPlaces = string.Format("{0:n2}", battler.Shares);
        hudLeftPanelTMPro.text = $"Net Worth($)\n{netWorthFiniteDecimalPlaces}\nShares\n{stockFiniteDecimalPlaces}";
    }
}
