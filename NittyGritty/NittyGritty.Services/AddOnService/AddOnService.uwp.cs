﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NittyGritty.Extensions;
using NittyGritty.Platform.Store;
using Windows.Services.Store;
using Windows.System;

namespace NittyGritty.Services
{
    public partial class AddOnService
    {
        private StoreContext context = null;

        async Task<ReadOnlyCollection<ConsumableAddOn>> PlatformGetConsumableAddOns(params string[] keys)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var consumables = new Collection<ConsumableAddOn>();

            // Specify the kinds of add-ons to retrieve.
            if (keys.Length == 0)
            {
                consumables.AddRange(_addOnsByKey.Values.OfType<ConsumableAddOn>());
            }
            else
            {
                foreach (var key in keys)
                {
                    if (_addOnsByKey[key] is ConsumableAddOn cao)
                    {
                        consumables.Add(cao);
                    }
                }
            }

            var queryResult = await context.GetStoreProductsAsync(new List<string>() { "Consumable" }, consumables.Select(c => c.Id));
            if (queryResult.ExtendedError == null)
            {
                foreach (var consumable in consumables)
                {
                    var product = queryResult.Products[consumable.Id];
                    var balanceResult = await context.GetConsumableBalanceRemainingAsync(product.StoreId);
                    consumable.Title = product.Title;
                    consumable.Description = product.Description;
                    consumable.Price = product.Price.FormattedPrice;
                    consumable.CustomData = product.Skus[0].CustomDeveloperData;
                    consumable.Balance = balanceResult.Status == StoreConsumableStatus.Succeeded ? balanceResult.BalanceRemaining : 0;
                }
            }

            return new ReadOnlyCollection<ConsumableAddOn>(consumables);
        }

        async Task<ReadOnlyCollection<DurableAddOn>> PlatformGetDurableAddOns(params string[] keys)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var durables = new Collection<DurableAddOn>();

            // Specify the kinds of add-ons to retrieve.
            if (keys.Length == 0)
            {
                durables.AddRange(_addOnsByKey.Values.OfType<DurableAddOn>());
            }
            else
            {
                foreach (var key in keys)
                {
                    if (_addOnsByKey[key] is DurableAddOn dao)
                    {
                        durables.Add(dao);
                    }
                }
            }

            var queryResult = await context.GetStoreProductsAsync(new List<string>() { "Durable" }, durables.Select(d => d.Id));
            if (queryResult.ExtendedError == null)
            {
                var license = await context.GetAppLicenseAsync();
                foreach (var durable in durables)
                {
                    var product = queryResult.Products[durable.Id];
                    if (!product.Skus[0].IsSubscription)
                    {
                        durable.Title = product.Title;
                        durable.Description = product.Description;
                        durable.Price = product.Price.FormattedPrice;
                        durable.CustomData = product.Skus[0].CustomDeveloperData;
                        durable.IsActive = license.AddOnLicenses.TryGetValue(product.StoreId, out var l) ? l.IsActive : false;
                    }
                }
            }

            return new ReadOnlyCollection<DurableAddOn>(durables);
        }

        async Task<ReadOnlyCollection<SubscriptionAddOn>> PlatformGetSubscriptionAddOns(params string[] keys)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var subscriptions = new Collection<SubscriptionAddOn>();

            // Specify the kinds of add-ons to retrieve.
            if (keys.Length == 0)
            {
                subscriptions.AddRange(_addOnsByKey.Values.OfType<SubscriptionAddOn>());
            }
            else
            {
                foreach (var key in keys)
                {
                    if (_addOnsByKey[key] is SubscriptionAddOn sao)
                    {
                        subscriptions.Add(sao);
                    }
                }
            }

            var queryResult = await context.GetStoreProductsAsync(new List<string>() { "Durable" }, subscriptions.Select(s => s.Id));
            if (queryResult.ExtendedError == null)
            {
                var license = await context.GetAppLicenseAsync();
                foreach (var subscription in subscriptions)
                {
                    var product = queryResult.Products[subscription.Id];
                    if (product.Skus[0].IsSubscription)
                    {
                        subscription.Title = product.Title;
                        subscription.Description = product.Description;
                        subscription.Price = product.Price.FormattedPrice;
                        subscription.CustomData = product.Skus[0].CustomDeveloperData;
                        subscription.BillingPeriod = product.Skus[0].SubscriptionInfo.BillingPeriod;
                        subscription.BillingPeriodUnit = (DurationUnit)product.Skus[0].SubscriptionInfo.BillingPeriodUnit;
                        subscription.TrialPeriod = product.Skus[0].SubscriptionInfo.TrialPeriod;
                        subscription.TrialPeriodUnit = (DurationUnit)product.Skus[0].SubscriptionInfo.TrialPeriodUnit;
                        subscription.IsActive = license.AddOnLicenses.TryGetValue(product.StoreId, out var l) ? l.IsActive : false;
                    }
                }
            }

            return new ReadOnlyCollection<SubscriptionAddOn>(subscriptions);
        }

        async Task<ReadOnlyCollection<UnmanagedConsumableAddOn>> PlatformGetUnmanagedConsumableAddOns(params string[] keys)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var consumables = new Collection<UnmanagedConsumableAddOn>();

            // Specify the kinds of add-ons to retrieve.
            if (keys.Length == 0)
            {
                consumables.AddRange(_addOnsByKey.Values.OfType<UnmanagedConsumableAddOn>());
            }
            else
            {
                foreach (var key in keys)
                {
                    if (_addOnsByKey[key] is UnmanagedConsumableAddOn ucao)
                    {
                        consumables.Add(ucao);
                    }
                }
            }

            var queryResult = await context.GetStoreProductsAsync(new List<string>() { "UnmanagedConsumable" }, consumables.Select(u => u.Id));
            if (queryResult.ExtendedError == null)
            {
                foreach (var consumable in consumables)
                {
                    var product = queryResult.Products[consumable.Id];
                    var balanceResult = await context.GetConsumableBalanceRemainingAsync(product.StoreId);
                    consumable.Title = product.Title;
                    consumable.Description = product.Description;
                    consumable.Price = product.Price.FormattedPrice;
                    consumable.CustomData = product.Skus[0].CustomDeveloperData;
                    consumable.Balance = balanceResult.Status == StoreConsumableStatus.Succeeded ? balanceResult.BalanceRemaining : 0;
                }
            }

            return new ReadOnlyCollection<UnmanagedConsumableAddOn>(consumables);
        }

        async Task<bool> PlatformIsActive(IActiveAddOn addOn)
        {
            if (!(addOn is AddOn storeAddOn))
            {
                throw new ArgumentNullException(nameof(addOn));
            }

            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            // Specify the kinds of add-ons to retrieve.
            var license = await context.GetAppLicenseAsync();
            return license.AddOnLicenses.TryGetValue(storeAddOn.Id, out var l) ? l.IsActive : false;
        }

        async Task<bool> PlatformIsAnyActive(IEnumerable<IActiveAddOn> addOns)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            var license = await context.GetAppLicenseAsync();
            foreach (var item in addOns)
            {
                if (item is AddOn addOn)
                {
                    var isActive = license.AddOnLicenses.TryGetValue(addOn.Id, out var l) ? l.IsActive : false;
                    if (isActive)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        async Task PlatformPurchase(AddOn addOn)
        {
            await PlatformPurchaseById(addOn.Id);
        }

        async Task PlatformPurchaseById(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            var purchaseResult = await context.RequestPurchaseAsync(storeId);
            switch (purchaseResult.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    break;
                case StorePurchaseStatus.AlreadyPurchased:
                    throw new Exception($"You already own this add-on.", purchaseResult.ExtendedError);
                case StorePurchaseStatus.NotPurchased:
                    throw new Exception($"The purchase did not complete or may have been cancelled.", purchaseResult.ExtendedError);
                case StorePurchaseStatus.NetworkError:
                case StorePurchaseStatus.ServerError:
                    throw new Exception($"The purchase was unsuccessful due to a server or network error.", purchaseResult.ExtendedError);
                default:
                    throw new Exception("The purchase was unsuccessful due to an unknown error", purchaseResult.ExtendedError);
            }
        }

        async Task<bool> PlatformTryPurchase(AddOn addOn)
        {
            return await PlatformTryPurchaseById(addOn.Id);
        }

        async Task<bool> PlatformTryPurchaseById(string storeId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }

            var purchaseResult = await context.RequestPurchaseAsync(storeId);
            switch (purchaseResult.Status)
            {
                case StorePurchaseStatus.Succeeded:
                    return true;
                case StorePurchaseStatus.AlreadyPurchased:
                case StorePurchaseStatus.NotPurchased:
                case StorePurchaseStatus.NetworkError:
                case StorePurchaseStatus.ServerError:
                default:
                    return false;
            }
        }

        async Task PlatformReportUnmanagedConsumableFulfillment(string key, string trackingId)
        {
            await PlatformUpdateConsumableBalance(key, 1, trackingId);
        }

        async Task PlatformUpdateConsumableBalance(string key, uint quantity, string trackingId)
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
            }
            var result = await context.ReportConsumableFulfillmentAsync(_addOnsByKey[key].Id, quantity, Guid.Parse(trackingId));
            switch (result.Status)
            {
                case StoreConsumableStatus.Succeeded:
                    break;
                case StoreConsumableStatus.InsufficentQuantity:
                    throw new Exception($"The fulfillment was unsuccessful because the remaining " +
                        $"balance is insufficient. Remaining balance: {result.BalanceRemaining}", result.ExtendedError);
                case StoreConsumableStatus.NetworkError:
                case StoreConsumableStatus.ServerError:
                    throw new Exception($"The fulfillment was unsuccessful due to a server or network error.", result.ExtendedError);
                default:
                    throw new Exception($"The fulfillment was unsuccessful due to an unknown error.", result.ExtendedError);
            }
        }

        async Task<bool> PlatformAccessFeature(IActiveAddOn addOn, Func<bool, Task> feature, bool conditionWhenFree)
        {
            if (addOn == null)
            {
                throw new ArgumentNullException(nameof(addOn));
            }

            var isActive = await PlatformIsActive(addOn);
            if (isActive || conditionWhenFree)
            {
                await feature.Invoke(isActive);
                return true;
            }
            return false;
        }

        async Task<bool> PlatformAccessFeature(IEnumerable<IActiveAddOn> addOns, Func<bool, Task> feature, bool conditionWhenFree)
        {
            if (addOns is null)
            {
                throw new ArgumentNullException(nameof(addOns));
            }

            var isActive = await PlatformIsAnyActive(addOns);
            if (isActive || conditionWhenFree)
            {
                await feature.Invoke(isActive);
                return true;
            }
            return false;
        }

        async Task<bool> PlatformAccessFeature(IActiveAddOn addOn, Action<bool> feature, bool conditionWhenFree)
        {
            if (addOn == null)
            {
                throw new ArgumentNullException(nameof(addOn));
            }

            var isActive = await PlatformIsActive(addOn);
            if (isActive || conditionWhenFree)
            {
                feature.Invoke(isActive);
                return true;
            }
            return false;
        }

        async Task<bool> PlatformAccessFeature(IEnumerable<IActiveAddOn> addOns, Action<bool> feature, bool conditionWhenFree)
        {
            if (addOns is null)
            {
                throw new ArgumentNullException(nameof(addOns));
            }

            var isActive = await PlatformIsAnyActive(addOns);
            if (isActive || conditionWhenFree)
            {
                feature.Invoke(isActive);
                return true;
            }
            return false;
        }

        async Task PlatformManageSubscriptions()
        {
            await Launcher.LaunchUriAsync(new Uri("https://account.microsoft.com/services"));
        }
    }
}
