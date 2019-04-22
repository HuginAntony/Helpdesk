using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Helpdesk.Core.Helpers
{
    public static class ExcelHelper
    {
        public static void ToExcel<T>(this IEnumerable<T> collection, string path, string workSheetName, bool useCustomHeaders)
        {
            var newFile = new FileInfo(path);

            using (var excelPackage = new ExcelPackage(newFile))
            {
                if (useCustomHeaders)
                    excelPackage.Workbook.Worksheets.Add(workSheetName).Cells["A1"].LoadWithCustomHeaders(collection).AutoFitColumns();
                else
                    excelPackage.Workbook.Worksheets.Add(workSheetName).Cells["A1"].LoadFromCollection(collection, true, TableStyles.Medium2).AutoFitColumns();

                excelPackage.Save();
            }
        }
        public static ExcelRangeBase LoadWithCustomHeaders<T>(this ExcelRangeBase excelRange, IEnumerable<T> list)
        {
            excelRange.LoadFromCollection(list, true);

            const int Row = 1;
            int columnsCount = excelRange.Worksheet.Cells.Count() / list.Count();
            for (int column = 1; column <= columnsCount; column++)
            {
                string incorrectHeader = excelRange.Worksheet.Cells[Row, column].Text;

                PropertyInfo[] Properties = typeof(T).GetProperties();
                foreach (PropertyInfo Property in Properties)
                {
                    if (incorrectHeader == Property.Name.Replace('_', ' '))
                    {
                        object[] DisplayAttributes = Property.GetCustomAttributes(typeof(DisplayAttribute), true);

                        if (DisplayAttributes.Length == 1)
                        {
                            excelRange.Worksheet.Cells[Row, column].Value = ((DisplayAttribute)(DisplayAttributes[0])).Name;
                            break;
                        }
                    }
                }
            }
            return excelRange;
        }
    }
}
