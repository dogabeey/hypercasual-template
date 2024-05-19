using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Dogabeey
{
    public class CurrencyManager : SingletonComponent<CurrencyManager>
    {
        [Header("Coin")]
        public TMP_Text coinText;
        public Transform coinTransform;
        public SpriteRenderer coinSpritePrefab;
        [Header("Premium Currency")]
        public TMP_Text currencyText;
        public Transform premiumCurrencyTransform;
        public SpriteRenderer premiumCurrencySpritePrefab;
        [Header("Animation Settings")]
        public float flightDuration = 0.1f;
        public float coinSpriteMultiplier = 10;

        public float Coin { 
            get => PlayerPrefs.GetFloat("Coin", 0);
            set
            {
                PlayerPrefs.GetFloat("Coin", value);
                UpdateCoinText();
            }
        }
        public float PremiumCurrency
        {
            get => PlayerPrefs.GetFloat("PremiumCurrency", 0); 
            set => PlayerPrefs.GetFloat("PremiumCurrency", value); 
        }

        private void Start()
        {
            UpdateCoinText();
        }

        public void AddCoin(int coinAmount, GameObject source = null)
        {
            if(source != null)
            {
                AddCoinAnimation(source.transform.position, coinTransform.position, coinAmount);
            }
        }
        public void AddPremiumCurrency(int premiumCurrencyAmount, GameObject source = null)
        {
            if (source != null)
            {
                AddCoinAnimation(source.transform.position, premiumCurrencyTransform.position, premiumCurrencyAmount);
            }
            PremiumCurrency += premiumCurrencyAmount;
        }
        
        private void AddCoinAnimation(Vector3 sourcePosition, Vector3  targetPosition, int coinAmount)
        {
            StartCoroutine(AddCoinAnimationCoroutine(sourcePosition, targetPosition, coinAmount));
        }
        private IEnumerator AddCoinAnimationCoroutine(Vector3 sourcePosition, Vector3 targetPosition, int coinAmount)
        {
            int coinSpriteAmount = Mathf.CeilToInt((Coin + coinAmount * coinSpriteMultiplier) / Coin);
            for (int i = 0; i < coinSpriteAmount; i++)
            {
                SpriteRenderer coinSprite = Instantiate(coinSpritePrefab, sourcePosition, Quaternion.identity);
                coinSprite.transform.SetParent(coinTransform);
                yield return coinSprite.transform.DOMove(targetPosition, flightDuration).SetEase(Ease.InOutQuad).WaitForCompletion();
                Coin += coinAmount / coinSpriteAmount;
                Destroy(coinSprite.gameObject);
            }

            yield break;
        }

        void UpdateCoinText()
        {
            coinText.text = Mathf.CeilToInt(Coin).ToString();
        }
    }
}
