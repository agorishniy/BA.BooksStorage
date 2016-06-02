using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BA.BooksStorage.Model.Model;
using Google.DataTable.Net.Wrapper;
using Row = System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Row;

namespace BA.BooksStorage.Utils
{
    public class HitUtils
    {
        public static string GoogleChartData(ICollection<Hit> hits, ICollection<HitChart> avgHits)
        {
            //let's instantiate the DataTable.
            var curDate = DateTime.UtcNow.Date;
            var dt = new Google.DataTable.Net.Wrapper.DataTable();
            dt.AddColumn(new Column(ColumnType.Date, "Day", "Day"));
            dt.AddColumn(new Column(ColumnType.Number, "Count", "Count"));
            dt.AddColumn(new Column(ColumnType.Number, "Average", "Average"));

            for (var i = 1; i < 31; i++)
            {
                Google.DataTable.Net.Wrapper.Row r = dt.NewRow();

                Hit curHit = hits.FirstOrDefault(h => h.Date == curDate);
                var curCount = curHit?.Count ?? 0;

                HitChart avgHit = avgHits.FirstOrDefault(h => h.Date == curDate);
                var avgCount = avgHit?.Count ?? 0;

                r.AddCellRange(new Cell[]
                {
                        new Cell(curDate),
                        new Cell(curCount),
                        new Cell(avgCount)
                });
                dt.AddRow(r);

                curDate = curDate.AddDays(-1);
            }

            //Let's create a Json string as expected by the Google Charts API.
            return dt.GetJson();
        }
    }

}
