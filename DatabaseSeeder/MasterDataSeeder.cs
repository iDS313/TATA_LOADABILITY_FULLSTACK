using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Loadability.DatabaseSeeder
{
    public class MasterDataSeeder : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            
          
            var Sku = new List<Sku>
            {
                new Sku{ SkuCode="14000000000319",SkuTitle="Tata Agni Lf 1Kg PPMH/SOUTH"},
                new Sku{ SkuCode="14000000000324",SkuTitle="Tata Agni Lf 500g PP MH/SOUTH"},
                new Sku{ SkuCode="14000000000329",SkuTitle="Tata Agni Lf 250g PPMH/SOUTH"},
                new Sku{ SkuCode="14000000000334",SkuTitle="Tata Agni Lf 100g PP MH/SOUTH"},
                new Sku{ SkuCode="14000000000446",SkuTitle="Tata Tea Pr Leaf 1kg Standipack Mah"},
                new Sku{ SkuCode="14000000000449",SkuTitle="Tata Tea Pr Leaf 100gm Polypack Mah"},
            };

            var Cfa = new List<Cfa> {
            new Cfa{ CfaLocation="JABALPUR",DepoCode="1132"},
            new Cfa{ CfaLocation="INDORE OLD",DepoCode="1143"},
            new Cfa{ CfaLocation="INDORE NEW",DepoCode="1225"},
            new Cfa{ CfaLocation="HYDRABAD",DepoCode="1160"},
            new Cfa{ CfaLocation="THANE",DepoCode="1135"},
            new Cfa{ CfaLocation="CUTTACK",DepoCode="1213"},
            };

            context.Cfa.AddRange(Cfa);
            context.Sku.AddRange(Sku);
          
            context.SaveChanges();
        }

    }

}