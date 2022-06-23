namespace WXRadio.WeatherManager.Product
{
    public class EmptyProductUpdateEventArgs : IProductUpdateEventArgs
    {
        public EmptyProductUpdateEventArgs(BaseProduct product)
        {
            this.Product = product;
        }
        public BaseProduct Product { get; set; }
    }
}
