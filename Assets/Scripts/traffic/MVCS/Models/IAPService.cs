using UnityEngine;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;
using System;

namespace Traffic.MVCS.Models
{
    public enum IAPType {
        AdditionalLevels,
        NoAdverts
    }

    public interface IAPService
    {
        bool ApplyCode(string code);

        bool IsBought(IAPType what);
        void PurchaseStart(IAPType what);

        bool GetProductPrice(IAPType what, out float price, out string currency);

        void RestorePurchases();

        PurshaseOk onPurchaseOk { get; set; }
        PurchaseFailed onPurchaseFailed { get; set; }
        RestorePurchasesFailed onRestorePurchaseFailed { get; set; }
    }

    public class IAPServiceDummy : IAPService
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public RestorePurchasesFailed onRestorePurchaseFailed { get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        public bool ApplyCode(string code)
        {
            return false;            
        }

        public bool GetProductPrice(IAPType what, out float price, out string currency)
        {
            price = 1;
            currency = "$";
            return true;
        }

        public void RestorePurchases()
        {
            onRestorePurchaseFailed.Dispatch();
            //PurchaseStart(IAPType.NoAdverts);
            //PurchaseStart(IAPType.AdditionalLevels);            
        }

        public bool IsBought(IAPType what)
        {
#if UNITY_WEBGL
            return true;
#endif
            return PlayerPrefs.GetInt("iap." + what.ToString(), 0) == 1;
        }

        public void PurchaseStart(IAPType what)
        {
            PlayerPrefs.SetInt("iap." + what.ToString(), 1);
            onPurchaseOk.Dispatch(what);  
        }
    }
}