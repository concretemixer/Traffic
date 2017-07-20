using UnityEngine;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;
using System;

namespace Traffic.MVCS.Models
{
    public class IAPServiceVK : IAPService
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
            currency = "";
            price = 1;
            switch (what)
            {
                case IAPType.Tries100: price = 7; break;
                case IAPType.Tries1000: price = 35; break;
            }
            
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
            return false;
        }

        public void PurchaseStart(IAPType what)
        {
            //PlayerPrefs.SetInt("iap." + what.ToString(), 1);
            //onPurchaseOk.Dispatch(what);  
            Application.ExternalCall("order", new string[] { what.ToString().ToLowerInvariant() });
        }
    }
}