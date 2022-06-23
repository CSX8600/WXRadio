using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Product;

namespace AdvisoryDisplay
{
    public partial class AdvisoryDisplayControl : UserControl
    {
        frmMap map;
        frmMap.StormDelegate MapStormUpdatedDelegate;
        frmMap.StormDelegate MapStormRemovedDelegate;

        public AdvisoryDisplayControl(Config config) : this()
        {
            ProductManager.INSTANCE.ProductAdded += ProductAdded;
            StormManager.INSTANCE.StormAdded += StormAdded;

            map = new frmMap(config.CenterX, config.CenterZ);
            MapStormUpdatedDelegate = map.GetUpdateStormDelegate();
            MapStormRemovedDelegate = map.GetDeleteStormDelegate();
        }

        protected AdvisoryDisplayControl()
        {
            InitializeComponent();
        }

        private void StormAdded(object sender, Storm e)
        {
            MapStormUpdatedDelegate(e);
            e.StormUpdated += StormUpdated;
            e.StormDeleted += StormDeleted;
        }

        private void StormDeleted(object sender, Storm e)
        {
            MapStormRemovedDelegate(e);
            e.StormUpdated -= StormUpdated;
            e.StormDeleted -= StormDeleted;
        }

        private void StormUpdated(object sender, Storm e)
        {
            MapStormUpdatedDelegate(e);
        }

        private void ProductAdded(object sender, BaseProduct e)
        {
            e.ProductRemoved += ProductRemoved;
            e.ProductUpdate += ProductUpdate;

            Invoke(new MethodInvoker(() =>
            {
                TreeNode productNode = new TreeNode(e.GetType().Name + " (" + e.ProductGuid + ")");
                productNode.Tag = e;

                TreeNode detailedInfoNode = new TreeNode();

                if (e is ICancellable && ((ICancellable)e).IsCancelled)
                {
                    detailedInfoNode.Text = ((ICancellable)e).GetCancelSummary();
                }
                else
                {
                    detailedInfoNode.Text = e.GetDetailedInformation();
                }

                productNode.Nodes.Add(detailedInfoNode);

                treAdvisories.Nodes.Add(productNode);
            }));
        }

        private void ProductUpdate(object sender, IProductUpdateEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                TreeNode advisoryNode = treAdvisories.Nodes.Cast<TreeNode>().First(n => n.Tag == e.Product);
                advisoryNode.Nodes.Clear();

                TreeNode detailNode = new TreeNode();

                if (e.Product is ICancellable && ((ICancellable)e.Product).IsCancelled)
                {
                    detailNode.Text = ((ICancellable)e.Product).GetCancelSummary();
                }
                else
                {
                    detailNode.Text = e.Product.GetDetailedInformation();
                }

                advisoryNode.Nodes.Add(detailNode);
            }));
        }

        private void ProductRemoved(object sender, BaseProduct e)
        {
            Invoke(new MethodInvoker(() =>
            {
                TreeNode advisoryNode = treAdvisories.Nodes.Cast<TreeNode>().First(n => n.Tag == e);
                advisoryNode.Remove();
            }));

            e.ProductUpdate -= ProductUpdate;
            e.ProductRemoved -= ProductRemoved;
        }

        private void mnuMap_Click(object sender, System.EventArgs e)
        {
            map.Show();
        }
    }
}
