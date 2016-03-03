using System;
using System.Collections.Generic;
using System.Linq;
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

        private IStoreController StoreController;             
        private IExtensionProvider StoreExtensionProvider;

        private Dictionary<IAPType, string> productIds = new Dictionary<IAPType,string>();
       
        public IAPServiceUnity()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            
            string storeName = "";
#if UNITY_ANDRIOD
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.traffic.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.traffic.no_ads")
            productIds.Add(IAPType.AdditionalLevels, "android.test.purchased");
            productIds.Add(IAPType.NoAdverts, "android.test.purchased");
            storeName = GooglePlay.Name;
#endif
#if UNITY_IOS
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.trafficstorm.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.trafficstorm.no_ads")
            storeName = AppleAppStore.Name;
#endif
#if UNITY_WINPHONE
            productIds.Add(IAPType.AdditionalLevels, "com.concretemixer.traffic.level_pack_1");
            productIds.Add(IAPType.NoAdverts, "com.concretemixer.traffi.no_ads")
            storeName = WindowsPhone8.Name;
#endif

            // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
            foreach (var key in productIds.Keys)
            {             
                builder.AddProduct(key.ToString(), ProductType.NonConsumable,
                    new IDs() { { productIds[key], storeName } });
                
            }
            UnityPurchasing.Initialize(this, builder);
            
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
            onPurchaseFailed.Dispatch(what);
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
        }
    
        public bool IsBought(IAPType what)
        {
            Product product = StoreController.products.WithID(productIds[what]);
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

        public void PurchaseStart(IAPType what)
        {
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {
                    // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                    Product product = StoreController.products.WithID(productIds[what]);

                    // If the look up found a product for this device's store and that product is ready to be sold ... 
                    if (product != null && product.availableToPurchase)
                    {
                        Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                        StoreController.InitiatePurchase(product);                        
                    }
                    // Otherwise ...
                    else
                    {
                        
                        onPurchaseFailed.Dispatch(what);  
                        // ... report the product look-up failure situation  
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                // Otherwise ...
                else
                {
                    onPurchaseFailed.Dispatch(what);  
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                onPurchaseFailed.Dispatch(what);  
                // ... by reporting any unexpected exception for later diagnosis.
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }


	}
}
