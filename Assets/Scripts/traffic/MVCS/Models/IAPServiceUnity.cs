using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEngine.Purchasing;

using Traffic.MVCS.Commands.Signals;

namespace Traffic.MVCS.Models
{
    class IAPServiceUnity : IAPService, IStoreListener
	{
        [Inject]
        public PurshaseOk onPurchaseOk { get; set; }

        [Inject]
        public PurchaseFailed onPurchaseFailed { get; set; }

        [Inject]
        public RestorePurchasesFailed onRestorePurchaseFailed { get; set; }

        private IStoreController StoreController;             
        private IExtensionProvider StoreExtensionProvider;

        private Dictionary<IAPType, string> productIds = new Dictionary<IAPType,string>();

        


        public IAPServiceUnity()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            
            string storeName = "";
#if UNITY_ANDROID
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.traffic.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.traffic.no_ads");
           // productIds.Add(IAPType.AdditionalLevels, "android.test.item_unavailable");
           // productIds.Add(IAPType.NoAdverts, "android.test.canceled");
            storeName = GooglePlay.Name;
#endif
#if UNITY_IOS
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.trafficstorm.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.trafficstorm.no_ads");
            storeName = AppleAppStore.Name;
#endif
#if UNITY_WINPHONE
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.traffic.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.traffi.no_ads");
            storeName = WindowsPhone8.Name;
#endif

            // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
            foreach (var key in productIds.Keys)
            {             
                builder.AddProduct(key.ToString(), ProductType.NonConsumable,
                    new IDs() { { productIds[key], storeName } });
                
            }
            UnityPurchasing.Initialize(this, builder);

            if (PlayerPrefs.GetInt("iap." + IAPType.AdditionalLevels, 0) == 1)
                PlayerPrefs.SetInt("iap." + IAPType.NoAdverts, 1);
        }

        public bool ApplyCode(string code)
        {
//            UnityEngine.Debug.Log(code);

            DateTime expireTime = DateTime.Now.AddDays(-1);

//#if UNITY_ANDROID            
            if (code == "299883")
                expireTime = new DateTime(2016, 6, 26, 23, 59, 59, DateTimeKind.Utc);
            if (code == "458109")
                expireTime = new DateTime(2016, 4, 25, 23, 59, 59, DateTimeKind.Utc);
            if (code == "862270")
                expireTime = new DateTime(2016, 4, 24, 23, 59, 59, DateTimeKind.Utc);
            if (code == "488910")
                expireTime = new DateTime(2016, 4, 23, 23, 59, 59, DateTimeKind.Utc);
            if (code == "389245")
                expireTime = new DateTime(2016, 4, 22, 23, 59, 59, DateTimeKind.Utc);
            if (code == "254493")
                expireTime = new DateTime(2016, 4, 21, 23, 59, 59, DateTimeKind.Utc);
            if (code == "454584")
                expireTime = new DateTime(2016, 4, 20, 23, 59, 59, DateTimeKind.Utc);
            if (code == "715871")
                expireTime = new DateTime(2016, 4, 19, 23, 59, 59, DateTimeKind.Utc);
            if (code == "998421")
                expireTime = new DateTime(2016, 4, 18, 23, 59, 59, DateTimeKind.Utc);
            if (code == "932337")
                expireTime = new DateTime(2016, 4, 17, 23, 59, 59, DateTimeKind.Utc);
            if (code == "167735")
                expireTime = new DateTime(2016, 4, 16, 23, 59, 59, DateTimeKind.Utc);
//#endif

            if (DateTime.Now < expireTime)
            {
                PlayerPrefs.SetInt("iap." + IAPType.AdditionalLevels.ToString(), 1);
                return true;
            }

            return false;
        }

        public bool GetProductPrice(IAPType what, out float price, out string currency)
        {
            price = 1000;
            currency = "Gold";

            if (!IsInitialized())
                return false;

            var product = StoreController.products.WithID(what.ToString());

            if (product==null)
                return false;

            currency = product.metadata.isoCurrencyCode;
            price = (float)product.metadata.localizedPrice;

            if (currency == "USD")
                currency = "$";
        //    if (currency == "EUR")
       //         currency = "€";

            return true;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            StoreExtensionProvider = extensions;
            
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
        {  
            

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            PlayerPrefs.SetInt("iap." + args.purchasedProduct.definition.id, 1);
            IAPType what = (IAPType)Enum.Parse(typeof(IAPType), args.purchasedProduct.definition.id, true);
            onPurchaseOk.Dispatch(what);        
            return PurchaseProcessingResult.Complete;
        }
        
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            IAPType what = (IAPType)Enum.Parse(typeof(IAPType), product.definition.id, true);
            onPurchaseFailed.Dispatch(what,failureReason.ToString());
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
        }
    
        public bool IsBought(IAPType what)
        {
            if (PlayerPrefs.GetInt("iap." + what.ToString(), 0) == 1)
                return true;

            if (!IsInitialized())
                return false;

            Product product = StoreController.products.WithID(what.ToString());
            if (product != null && product.availableToPurchase)
            {
                return PlayerPrefs.GetInt("iap." + product.definition.id, 0) == 1;
            }
            return false;            
        }

        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return StoreController != null && StoreExtensionProvider != null;
        }


        // Restore purchases previously made by this customer. Some platforms automatically restore purchases. Apple currently requires explicit purchase restoration for IAP.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                onRestorePurchaseFailed.Dispatch();
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");

                // Fetch the Apple store-specific subsystem.
                IAppleExtensions apple = StoreExtensionProvider.GetExtension<IAppleExtensions>();
                
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) =>
                {
                    if (!result)
                        onRestorePurchaseFailed.Dispatch();
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                onRestorePurchaseFailed.Dispatch();
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }
        

        public void PurchaseStart(IAPType what)
        {
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {
                    // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                    Product product = StoreController.products.WithID(what.ToString());

                    // If the look up found a product for this device's store and that product is ready to be sold ... 
                    if (product != null && product.availableToPurchase)
                    {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        StoreController.InitiatePurchase(product);                        
                    }
                    // Otherwise ...
                    else
                    {
                        
                        onPurchaseFailed.Dispatch(what, "Product not available");  
                        // ... report the product look-up failure situation  
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                // Otherwise ...
                else
                {
                    onPurchaseFailed.Dispatch(what, "Not initialized");  
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                onPurchaseFailed.Dispatch(what, "E: "+e.Message);  
                // ... by reporting any unexpected exception for later diagnosis.
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }


	}
}
