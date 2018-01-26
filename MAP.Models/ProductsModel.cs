using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{
    public class ProductsModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ProductCategory { get; set; }
        public int Status { get; set; }
        public string efDate { get; set; }
        public string Spec { get; set; }
        public decimal Price { get; set; }

        public string ProductPriceMapXML { get; set; }

        private List<ProductPriceMapModel> priceMap = new List<ProductPriceMapModel>();

        public List<ProductPriceMapModel> ProductPriceMap
        {
            get { return priceMap; }
            set { priceMap = value; }
        }
    }


    public class ProductPriceMapModel
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }

        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public decimal Price { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }         

        public string WareHouseOptions { get; set; }
    }
}
