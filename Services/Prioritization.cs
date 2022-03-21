using Loadability.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Services
{
    public  class Prioritization
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
                        Priority.IsPlaned = false;
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
                        pri.IsPlaned = false;
                        pri.PlanDate = PlanDate;
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