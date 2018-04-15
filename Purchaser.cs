using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{
    public static Purchaser Instance { set; get; }

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string DisableAds = "1.premium_bonus";
    //public static string kProductIDConsumable = "consumable";
    //public static string kProductIDNonConsumable = "nonconsumable";
    //public static string kProductIDSubscription = "subscription";
    //private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(DisableAds, ProductType.NonConsumable);

        //builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        //builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);
        //builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
            
            //{ kProductNameGooglePlaySubscription, GooglePlay.Name },
        //});

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyConsumable()
    {
        //BuyProductID(kProductIDConsumable);
    }

    public void BuyNonConsumable()
    {
        BuyProductID(DisableAds);
    }

    public void BuySubscription()
    {
        //BuyProductID(kProductIDSubscription);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId); 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            { 
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {

        Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        
        if (String.Equals(args.purchasedProduct.definition.id, DisableAds, StringComparison.Ordinal))
        {
            GameMaster.Instance.disableAds = true;
            GameMaster.Instance.Save();
            GameMaster.Instance.Load();
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
            return PurchaseProcessingResult.Complete;
        }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase Failed");
    }
}


