using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Services
{
    public class Prioritization
    {
        public static bool Prioritize(DateTime PlanDate , ApplicationDbContext _ctx)
        {
            var dp = _ctx.DailyPlan.Where(x => x.PlanDate == PlanDate.Date).GroupBy(x => x.PriorityQty).OrderByDescending(x => x.Key).ToList();
            int Rank = 1;
            foreach (var i in dp)
            {

                var j = i.OrderByDescending(x => x.SHQ);
                foreach (var k in j)
                {
                    var pri = _ctx.Priority.Where(x => x.CfaId == k.CfaId && x.SkuId == k.SkuId && x.PlanDate == PlanDate).FirstOrDefault();
                    if (pri == null)
                    {
                        var Priority = new Priority();
                        Priority.SkuId = k.SkuId;
                        Priority.CfaId = k.CfaId;
                        Priority.PlanDate = PlanDate;
                        Priority.SHQ = k.SHQ;
                        Priority.IsLoaded = false;
                        Priority.IsCompared = false;
                        if (Priority.SHQ > 1)
                        {
                            Priority.Rank = Rank;
                            Rank++;
                        }
                        else
                        {
                            Priority.Rank = (-1) * Rank;
                        }
                        _ctx.Priority.Add(Priority);
                        _ctx.SaveChanges();

                    }
                    else
                    {
                        pri.SHQ = k.SHQ;
                        pri.IsLoaded = false;
                        pri.PlanDate = PlanDate;
                        pri.IsCompared = false;
                        var stock = _ctx.StockDetails.Where(y => y.SkuId == pri.SkuId).FirstOrDefault();
                        var prdetails = _ctx.PrDetails.Where(bn => bn.CfaId == pri.CfaId && bn.SkuId == pri.SkuId && bn.PrDate == pri.PlanDate).FirstOrDefault();
                        if (stock != null)
                        {
                            stock.StartQty = stock.StartQty + pri.FinalQty;
                            stock.AvailableQty = stock.AvailableQty - pri.FinalQty;
                            pri.FinalQty = 0;
                            pri.InStock = true;

                        }
                        if (prdetails != null)
                        {
                            prdetails.PrQty = prdetails.PrQty + pri.QtyFromPr;
                            prdetails.SuppliedQty = prdetails.SuppliedQty - pri.QtyFromPr;
                            pri.QtyFromPr = 0;
                            pri.InPr = false;
                            pri.IsLoaded = false;
                        }
                        if (pri.SHQ > 1)
                        {
                            pri.Rank = Rank;
                            Rank++;
                        }
                        else
                        {
                            pri.Rank = (-1) * Rank;
                        }
                        _ctx.SaveChanges();
                    }
                }

            }
            return true;
        }
    }
}