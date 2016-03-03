﻿using System;
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

            productIds.Add(IAPType.AdditionalLevels, "android.test.purchased");
            productIds.Add(IAPType.NoAdverts, "android.test.purchased");

            // Add a product to sell / restore by way of its identifier, associating the general identifier with its store-specific identifiers.
            foreach (var key in productIds.Keys)
            {             
                builder.AddProduct(key.ToString(), ProductType.NonConsumable, 
                    new IDs() { { productIds[key], GooglePlay.Name } } );
                
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
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, "", StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));//If the consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                
            }
           
            return PurchaseProcessingResult.Complete;
        }
        
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing this reason with the user.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",product.definition.storeSpecificId, failureReason));
        }
    
        public bool IsBought(IAPType what)
        {
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
                        // ... report the product look-up failure situation  
                        Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                // Otherwise ...
                else
                {
                    // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or retrying initiailization.
                    Debug.Log("BuyProductID FAIL. Not initialized.");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                // ... by reporting any unexpected exception for later diagnosis.
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }


	}
}
