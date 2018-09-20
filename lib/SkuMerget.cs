using image_02.Models;
using System;

namespace image_02.lib
{
    public static class SkuMerget
    {
        /*H0783*
        /*LW1YE0000*/
        /*075922323230011BLFULI*/
        /*Af*/

        public static Sku getSku(string sku)
        {
            Sku _sku = new Sku();
            _sku.Type = Convert.ToString(sku.Substring(0, 1));

            _sku.Width = Convert.ToInt32(sku.Substring(1, 4));
            _sku.Base = Convert.ToString(sku.Substring(5, 9));
            _sku.Product = Convert.ToString(_sku.Base.Substring(0, 3));
            _sku.Art = Convert.ToString(sku.Substring(14, 21));
            return _sku;
        }
    }
}