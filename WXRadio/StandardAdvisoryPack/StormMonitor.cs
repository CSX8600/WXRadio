using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StandardAdvisoryPack.Product;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Extensions;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager.Utility;

namespace StandardAdvisoryPack
{
    public class StormMonitor
    {
        private Dictionary<Storm, List<BaseProduct>> productsForStorm = new Dictionary<Storm, List<BaseProduct>>();
        public StormMonitor()
        {
            StormManager.INSTANCE.StormAdded += StormAdded;
        }

        private void StormAdded(object sender, Storm e)
        {
            e.StormUpdated += StormUpdated;
            e.StormDeleted += StormDeleted;
            productsForStorm.Add(e, new List<BaseProduct>());

            GenerateNewProductForStorm(e);
        }

        private void GenerateNewProductForStorm(Storm e)
        {
            if (e.StormTrack.Count < 5)
            {
                return;
            }

            if (e.Strength >= Storm.Strengths.Thunderstorm)
            {
                ConsiderSevereThunderstormWatch(e);
            }

            if (e.Strength >= Storm.Strengths.SevereThunderstorm)
            {
                ConsiderSevereThunderstormWarning(e);
            }

            if (e.Strength >= Storm.Strengths.SevereThunderstormWithHail)
            {
                ConsiderTornadoWatch(e);
            }

            if (e.Strength >= Storm.Strengths.TornadoF0)
            {
                ConsiderTornadoWarning(e);
            }
        }

        private void StormUpdated(object sender, Storm e)
        {
            if (productsForStorm.ContainsKey(e))
            {
                foreach(BaseProduct product in productsForStorm[e])
                {
                    if (product is SevereThunderstormWatch)
                    {
                        SevereThunderstormWatch severeThunderstormWatch = (SevereThunderstormWatch)product;
                        if (!severeThunderstormWatch.IsCancelled &&
                                (e.Strength < Storm.Strengths.Thunderstorm ||
                                !Util.CoordinateIsInPolygon(e.Coordinate, product.GetPolygonCoordinates())))
                        {
                            severeThunderstormWatch.IsCancelled = true;
                        }
                    }

                    if (product is SevereThunderstormWarning)
                    {
                        SevereThunderstormWarning severeThunderstormWarning = (SevereThunderstormWarning)product;
                        if (!severeThunderstormWarning.IsCancelled &&
                            (e.Strength < Storm.Strengths.SevereThunderstorm ||
                            !Util.CoordinateIsInPolygon(e.Coordinate, product.GetPolygonCoordinates())))
                        {
                            severeThunderstormWarning.IsCancelled = true;
                        }
                    }

                    if (product is TornadoWatch)
                    {
                        TornadoWatch tornadoWatch = (TornadoWatch)product;
                        if (!tornadoWatch.IsCancelled &&
                            (e.Strength < Storm.Strengths.SevereThunderstormWithHail ||
                            !Util.CoordinateIsInPolygon(e.Coordinate, product.GetPolygonCoordinates())))
                        {
                            tornadoWatch.IsCancelled = true;
                        }
                    }

                    if (product is TornadoWarning)
                    {
                        TornadoWarning tornadoWarning = (TornadoWarning)product;
                        if (!tornadoWarning.IsCancelled &&
                            (e.Strength < Storm.Strengths.TornadoF0 ||
                            !Util.CoordinateIsInPolygon(e.Coordinate, product.GetPolygonCoordinates())))
                        {
                            tornadoWarning.IsCancelled = true;
                        }
                    }
                }
            }

            GenerateNewProductForStorm(e);
        }

        private void StormDeleted(object sender, Storm e)
        {
            if (productsForStorm.ContainsKey(e))
            {
                foreach(BaseProduct baseProduct in productsForStorm[e])
                {
                    ProductManager.INSTANCE.RemoveProduct(baseProduct);
                }
                productsForStorm.Remove(e);
            }
        }

        private void ConsiderSevereThunderstormWatch(Storm storm)
        {
            if (!productsForStorm[storm].Any(p => p is SevereThunderstormWatch && !((SevereThunderstormWatch)p).IsCancelled))
            {
                SevereThunderstormWatch severeThunderstormWatch = new SevereThunderstormWatch(storm);
                ProductManager.INSTANCE.AddProduct(severeThunderstormWatch);
                productsForStorm[storm].Add(severeThunderstormWatch);
            }
        }

        private void ConsiderSevereThunderstormWarning(Storm storm)
        {
            if (!productsForStorm[storm].Any(p => p is SevereThunderstormWarning && !((SevereThunderstormWarning)p).IsCancelled))
            {
                SevereThunderstormWarning severeThunderstormWarning = new SevereThunderstormWarning(storm);
                ProductManager.INSTANCE.AddProduct(severeThunderstormWarning);
                productsForStorm[storm].Add(severeThunderstormWarning);
            }
        }

        private void ConsiderTornadoWatch(Storm storm)
        {
            if (!productsForStorm[storm].Any(p => p is TornadoWatch && !((TornadoWatch)p).IsCancelled))
            {
                TornadoWatch tornadoWatch = new TornadoWatch(storm);
                ProductManager.INSTANCE.AddProduct(tornadoWatch);
                productsForStorm[storm].Add(tornadoWatch);
            }
        }

        private void ConsiderTornadoWarning(Storm storm)
        {
            if (!productsForStorm[storm].Any(p => p is TornadoWarning && !((TornadoWarning)p).IsCancelled))
            {
                TornadoWarning tornadoWarning = new TornadoWarning(storm);
                ProductManager.INSTANCE.AddProduct(tornadoWarning);
                productsForStorm[storm].Add(tornadoWarning);
            }
        }
    }
}
