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

    public GameObject youLosePanel;
    public Image youLosePanelImage;
    public Image youLoseTextPanelImage;
    public TextMeshProUGUI youLoseTextPanelTMPro;

    public GameObject gotOutPanel;
    public Image gotOutPanelImage;
    public Image gotOutTextPanelImage;
    public TextMeshProUGUI gotOutTextPanelTMPro;

    Color colorHudText = new Color32(0,255,0,255);
    Color colorHudBackground = new Color32(0,0,0,255);
    Color colorInvisible = new Color32(255,255,255,0);

    Color colorYouLoseMostText = new Color32(255,0,0,255);
    // Note: The Play Again line will have hardcoded color.
    Color colorYouLoseBackground = new Color32(0, 43, 54, 255);

    Color colorGotOutPositiveText = new Color32(0,255,0,255);
    Color colorGotOutNegativeText = new Color32(255,255,0,255);
    Color colorGotOutBackground = new Color32(0, 43, 54, 255);

    string playAgainString = "<b><color=#268bd2>Press P to play again</color></b>";

    // Start is called before the first frame update
    void Start()
    {
        SetHud();
        SetUpYouLosePanel();
        SetUpGotOutPanel();
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

    void SetUpYouLosePanel() {
        youLosePanelImage.color = colorYouLoseBackground;
        youLoseTextPanelImage.color = colorInvisible;
        youLoseTextPanelTMPro.color = colorYouLoseMostText;
        youLoseTextPanelTMPro.text = "You are <b>BANKRUPT</b>!\nYou <b>FAIL</b> at finance!\nGo play video games and not the stock market!\n\n" + playAgainString;
    }

    public void ShowYouLosePanel(bool show) {
        youLosePanel.SetActive(show);
    }

    void SetUpGotOutPanel() {
        gotOutPanelImage.color = colorGotOutBackground;
        gotOutTextPanelImage.color = colorInvisible;
    }

    public void HideGotOutPanel() {
        gotOutPanel.SetActive(false);
    }

    public void ShowGotOutPanel(float finalNetWorth) {
        var gains = finalNetWorth - Battler.initialNetWorth;
        var absGains = Mathf.Abs(gains);
        var absGainsDisplayable = string.Format("{0:n0}", absGains);
        if (gains > 0) {
            gotOutTextPanelTMPro.color = colorGotOutPositiveText;
            gotOutTextPanelTMPro.text = $"You got out with gains of\n<b>${absGainsDisplayable}</b>.\nYou are a stock market winner!\n\n" + playAgainString;
        } else {
            gotOutTextPanelTMPro.color = colorGotOutNegativeText;
            gotOutTextPanelTMPro.text = $"You got out with losses of\n<b>${absGainsDisplayable}</b>.\nAt least you didn't lose it all\n\n" + playAgainString;
        }
        gotOutPanel.SetActive(true);
    }

    public void UpdateStockPriceDisplay() {
        var stockPriceFiniteDecimalPlaces = Runner_GameScene.StockPrice.ToString("F2");
        hudUpperCenterPanelTMPro.text = $"Stock Price\n{stockPriceFiniteDecimalPlaces}";
    }

    public void UpdatePlayerStatsDisplay(Battler battler) {
        if (battler == null) {
            var netWorthFiniteDecimalPlaces = string.Format("{0:n0}", Battler.initialNetWorth);
            hudLeftPanelTMPro.text = $"Net Worth($)\n{netWorthFiniteDecimalPlaces}\nShares\nTBD";
        } else {
            // Assumption: Only the player's battler will be passed in,
            // not any other battlers.
            var netWorthFiniteDecimalPlaces = string.Format("{0:n0}", battler.netWorth);
            var stockFiniteDecimalPlaces = string.Format("{0:n2}", battler.Shares);
            hudLeftPanelTMPro.text = $"Net Worth($)\n{netWorthFiniteDecimalPlaces}\nShares\n{stockFiniteDecimalPlaces}";
        }
    }

    public void SetRightPanelToInstructions() {
        hudRightPanelTMPro.text = "<color=#268bd2>Press A to join the Azure team</color>\n<color=#ff0000>Press D to join the reD team</color>\n";
    }

    public void SetRightPanelToNoninstructions() {
        hudRightPanelTMPro.text = "";
    }
}
