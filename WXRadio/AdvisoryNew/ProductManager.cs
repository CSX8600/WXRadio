using System;
using System.Collections.Generic;
using System.Linq;
using WXRadio.WeatherManager.Product;

namespace WXRadio.WeatherManager
{
    public class ProductManager
    {
        public event EventHandler<BaseProduct> ProductAdded;

        private Dictionary<Guid, BaseProduct> _productsByID = new Dictionary<Guid, BaseProduct>();

        private static ProductManager _instance;
        public static ProductManager INSTANCE
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductManager();
                }

                return _instance;
            }
        }

        private ProductManager() { }

        public void AddProduct(BaseProduct baseProduct)
        {
            if (_productsByID.ContainsKey(baseProduct.ProductGuid))
            {
                return;
            }

            _productsByID.Add(baseProduct.ProductGuid, baseProduct);
            ProductAdded?.Invoke(this, baseProduct);
        }

        public void RemoveProduct(Guid productID)
        {
            if (!_productsByID.ContainsKey(productID))
            {
                return;
            }

            _productsByID[productID].RemoveProduct();
            _productsByID.Remove(productID);
        }

        public void RemoveProduct(BaseProduct product)
        {
            List<Guid> keys = _productsByID.Keys.Where(key => _productsByID[key] == product).ToList();
            foreach(Guid key in keys)
            {
                RemoveProduct(key);
            }
        }

        public IReadOnlyCollection<BaseProduct> GetProducts()
        {
            return _productsByID.Values;
        }
    }
}
