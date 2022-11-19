using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Used to simplify and beautify casting an object to a type.
        /// </summary>
        /// <typeparam name="T">Type to be casted</typeparam>
        /// <param name="obj">Object to cast</param>
        /// <returns>Casted object</returns>
        public static T As<T>(this object obj)
            where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.TypeCode)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Check if an item is in a list.
        /// </summary>
        /// <param name="item">Item to check</param>
        /// <param name="list">List of items</param>
        /// <typeparam name="T">Type of the items</typeparam>
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }


        /// <summary>
        /// Converts given object to a value excel file.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        //public static byte[] ToExcel<T>(this T model, string title)
        //    where T : class
        //{

        //    var properties = model.GetType().GetProperties().Where(x => x.IsDefined(typeof(ExcelReaderCellAttribute), false)).ToList();

        //    var workbook = new XSSFWorkbook();
        //    var myFont = (XSSFFont)workbook.CreateFont();
        //    myFont.FontHeightInPoints = 11;
        //    myFont.FontName = "Arial";


        //    // Defining a border
        //    var borderedCellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
        //    borderedCellStyle.SetFont(myFont);
        //    borderedCellStyle.BorderLeft = BorderStyle.Medium;
        //    borderedCellStyle.BorderTop = BorderStyle.Medium;
        //    borderedCellStyle.BorderRight = BorderStyle.Medium;
        //    borderedCellStyle.BorderBottom = BorderStyle.Medium;
        //    borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

        //    ISheet Sheet = workbook.CreateSheet(title);
        //    //Create The Headers of the excel
        //    IRow HeaderRow = Sheet.CreateRow(0);

        //    for (int i = 0; i < properties.Count; i++)
        //    {
        //        var data = properties[i];
        //        CreateCell(HeaderRow, i, data.Name, borderedCellStyle);
        //    }

        //    // Auto sized all the affected columns
        //    int lastColumNum = Sheet.GetRow(0).LastCellNum;
        //    for (int i = 0; i <= lastColumNum; i++)
        //    {
        //        Sheet.AutoSizeColumn(i);
        //        GC.Collect();
        //    }

        //    // output the XLSX file
        //    using (var ms = new MemoryStream())
        //    {
        //        workbook.Write(ms, leaveOpen: true);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        return ms.ToArray();
        //    }

        //}

        //private static void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
        //{
        //    ICell Cell = CurrentRow.CreateCell(CellIndex);
        //    Cell.SetCellValue(Value);
        //    Cell.CellStyle = Style;
        //}
    }
}
