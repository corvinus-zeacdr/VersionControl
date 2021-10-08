using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Gyak4_ZEACDR
{
    public partial class Form1 : Form
    {
        private int _million = (int)Math.Pow(10, 6);

        RealEstateEntities context = new RealEstateEntities();
        List<Flat> Flats;

        Excel.Application xlApp; // A Microsoft Excel alkalmazás
        Excel.Workbook xlWB; // A létrehozott munkafüzet
        Excel.Worksheet xlSheet; // Munkalap a munkafüzeten belül
        //string[] headers;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.DataSource = Flats;
            CreateExcel();
        }

        private void LoadData()
        {
            Flats = context.Flats.ToList();
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWB.ActiveSheet;
                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string hiba = string.Format("Error:¸{0}\nLine:{1}", ex.Message, ex.Source);
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }

        }

        private void CreateTable()
        {
            string[] headers = new string[] {
             "Kód",
             "Eladó",
             "Oldal",
             "Kerület",
             "Lift",
             "Szobák száma",
             "Alapterület (m2)",
             "Ár (mFt)",
             "Négyzetméter ár (Ft/m2)"};


            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }

            object[,] values = new object[Flats.Count, headers.Length];

            int counter = 0;
            int floorColumn = 6;
            foreach (Flat f in Flats)
            {
                values[counter, 0] = f.Code;
                values[counter, 1] = f.Vendor;
                values[counter, 2] = f.Side;
                values[counter, 3] = f.District;
                values[counter, 4] = f.Elevator ? "Van" : "Nincs";
                values[counter, 5] = f.NumberOfRooms;
                values[counter, floorColumn] = f.FloorArea;
                values[counter, 7] = f.Price;
                values[counter, 8] = string.Format("={0}/{1}*{2}",
                    "H" + (counter + 2).ToString(),
                    GetCell(counter + 2, floorColumn + 1),
                    _million.ToString());

                counter++;
            }

            var range = xlSheet.get_Range(
                        GetCell(2, 1),
                        GetCell(1 + values.GetLength(0), values.GetLength(1)));
                range.Value2 = values;


                Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
                headerRange.Font.Bold = true;
                headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                headerRange.EntireColumn.AutoFit();
                headerRange.RowHeight = 40;
                headerRange.Interior.Color = Color.LightBlue;
                headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

                int lastRowID = xlSheet.UsedRange.Rows.Count;
                Excel.Range completeTableRange = xlSheet.get_Range(GetCell(1, 1), GetCell(lastRowID, headers.Length));
                completeTableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

                Excel.Range firstColumn = xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), 1));
                firstColumn.Font.Bold = true;
                firstColumn.Interior.Color = Color.LightYellow;

                Excel.Range lastColumn = xlSheet.get_Range(GetCell(2, values.GetLength(1)), GetCell(1 + values.GetLength(0), values.GetLength(1)));
                lastColumn.Interior.Color = Color.Green;
                lastColumn.NumberFormat = "###,###.00";

        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        //private void FormatTable()
        //{
        //    Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
        //    headerRange.Font.Bold = true;
        //    headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        //    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
        //    headerRange.EntireColumn.AutoFit();
        //    headerRange.RowHeight = 40;
        //    headerRange.Interior.Color = Color.LightBlue;
        //    headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

        //    int lastRowID = xlSheet.UsedRange.Rows.Count;
        //    Excel.Range completeTableRange = xlSheet.get_Range(GetCell(1, 1), GetCell(lastRowID, headers.Length));
        //    completeTableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
        //}

    }
}
