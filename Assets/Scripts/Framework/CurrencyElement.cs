using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dogabeey
{

    public class CurrencyElement : MonoBehaviour
    {
        public Transform currencyTransform;
        public TMP_Text currencyText;
        public Image currencyIcon;

        internal CurrencyModel refCurrency;

        private void OnEnable()
        {
            EventManager.StartListening(Const.GameEvents.CURRENCY_CHANGED, OnCurrencyChanged);
        }
        private void OnDisable()
        {
            EventManager.StopListening(Const.GameEvents.CURRENCY_CHANGED, OnCurrencyChanged);
        }

        public void OnCurrencyChanged(EventParam param)
        {
            if (param.currencyModel == refCurrency)
            {
                UpdateCurrencyUI(refCurrency);
            }
        }
        public void UpdateCurrencyUI(CurrencyModel currency)
        {
            refCurrency = currency;
            currencyIcon.sprite = CurrencyManager.Instance.GetCurrencySprite(currency);
            currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(currency).ToString();
        }
    }
}
