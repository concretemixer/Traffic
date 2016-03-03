using UnityEngine;
using Traffic.MVCS.Commands.Signals;
using System.Threading;
using Traffic.Components;

namespace Traffic.MVCS.Models
{
    public enum IAPType {
        AdditionalLevels,
        NoAdverts
    }

    public interface IAPService
    {
        bool IsBought(IAPType what);
        void PurchaseStart(IAPType what);

        PurshaseOk onPurchaseOk { get; set; }
        PurchaseFailed onPurchaseFailed { get; set; }
    }

    public class IAPServiceDummy :  IAPService 
    {
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject(EntryPoint.Container.Stage)]
        public GameObject stage { get; set; }

        public bool IsBought(IAPType what)
        {
            return PlayerPrefs.GetInt("iap." + what.ToString(), 0) == 1;
        }

        public void PurchaseStart(IAPType what)
        {
            onPurchaseFailed.Dispatch();  
        }

        public void purchaseOkDispatch()
        {
            onPurchaseOk.Dispatch();            
        }
    }
}