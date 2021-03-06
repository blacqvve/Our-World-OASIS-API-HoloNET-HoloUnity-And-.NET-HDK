﻿
using System.Collections.Generic;
using System.Linq;

namespace NextGenSoftware.OASIS.API.Core
{
    public class ProviderManager 
    {
        private static List<IOASISProvider> _registeredProviders = new List<IOASISProvider>();

        public static ProviderType CurrentStorageProviderType { get; private set; }

        public static IOASISStorage CurrentStorageProvider { get; private set; } //TODO: Need to work this out because in future there can be more than one provider active at a time.

        

        //TODO: In future the registered providers will be dynamically loaded from MEF by watching a hot folder for compiled provider dlls (and other ways in future...)
        public static void RegisterProvider(IOASISProvider provider)
        {
            if (!_registeredProviders.Any(x => x.ProviderName == provider.ProviderName))
                _registeredProviders.Add(provider);
        }

        public static void RegisterProviders(List<IOASISProvider> providers)
        {
            foreach (IOASISProvider provider in providers)
                RegisterProvider(provider);
        }

        public static void UnRegisterProvider(IOASISProvider provider)
        {
            _registeredProviders.Remove(provider);
        }

        public static void UnRegisterProviders(List<IOASISProvider> providers)
        {
            foreach (IOASISProvider provider in providers)
                _registeredProviders.Remove(provider);
        }

        public static List<IOASISProvider> GetAllProviders()
        {
            return _registeredProviders;
        }

        public static List<IOASISProvider> GetProvidersOfCategory(ProviderCategory category)
        {
            return _registeredProviders.Where(x => x.ProviderCategory == category).ToList();
        }

        public static List<IOASISStorage> GetStorageProviders()
        {
            List<IOASISStorage> storageProviders = new List<IOASISStorage>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Storage || x.ProviderCategory == ProviderCategory.StorageAndNetwork).ToList())
                storageProviders.Add((IOASISStorage)provider);  

            return storageProviders;
        }

        public static List<IOASISNET> GetNetworkProviders()
        {
            List<IOASISNET> networkProviders = new List<IOASISNET>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Network || x.ProviderCategory == ProviderCategory.StorageAndNetwork).ToList())
                networkProviders.Add((IOASISNET)provider);

            return networkProviders;
        }

        public static List<IOASISRenderer> GetRendererProviders()
        {
            List<IOASISRenderer> rendererProviders = new List<IOASISRenderer>();

            foreach (IOASISProvider provider in _registeredProviders.Where(x => x.ProviderCategory == ProviderCategory.Renderer).ToList())
                rendererProviders.Add((IOASISRenderer)provider);

            return rendererProviders;
        }

        public static IOASISProvider GetProvider(ProviderType type)
        {
            return _registeredProviders.FirstOrDefault(x => x.ProviderType == type);
        }

        public static IOASISStorage GetStorageProvider(ProviderType type)
        {
            return (IOASISStorage)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Storage);
        }

        public static IOASISNET GetNetworkProvider(ProviderType type)
        {
            return (IOASISNET)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Network);
        }

        public static IOASISRenderer GetRendererProvider(ProviderType type)
        {
            return (IOASISRenderer)_registeredProviders.FirstOrDefault(x => x.ProviderType == type && x.ProviderCategory == ProviderCategory.Renderer);
        }

        // Highly recommend this one is used.
        public static IOASISProvider GetAndActivateProvider(ProviderType type)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == type);

            if (provider != null)
                provider.ActivateProvider();

            return provider;
        }

        public static bool IsProviderRegistered(IOASISProvider provider)
        {
            return _registeredProviders.Any(x => x.ProviderName == provider.ProviderName);
        }

        //TODO: In future more than one StorageProvider will be active at a time so we need to work out how to handle this...
        public static void SwitchCurrentStorageProvider(ProviderType providerType)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == providerType);

            if (provider != null && (provider.ProviderCategory == ProviderCategory.Storage || provider.ProviderCategory == ProviderCategory.StorageAndNetwork))
            {
                ProviderManager.CurrentStorageProviderType = providerType;
                ProviderManager.CurrentStorageProvider = (IOASISStorage)provider;
                ProviderManager.CurrentStorageProvider.ActivateProvider();
            }
        }

        public static void ActivateProvider(ProviderType type)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == type);

            if (provider != null)
                provider.ActivateProvider();
        }

        public static void DeaAtivateProvider(ProviderType type)
        {
            IOASISProvider provider = _registeredProviders.FirstOrDefault(x => x.ProviderType == type);

            if (provider != null)
                provider.DeActivateProvider();
        }
    }
}
