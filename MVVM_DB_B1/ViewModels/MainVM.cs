using Aspose.Cells;
using Microsoft.Win32;
using MVVM_DB_B1.Data.ExelModels;
using MVVM_DB_B1.Data.Interfaces;
using MVVM_DB_B1.Data.Models;
using MVVM_DB_B1.Data.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MVVM_DB_B1.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        public ICommand LoadExelFile
        {
            get
            {
                return new DelegateComand(obj => Task.Factory.StartNew(() =>
                {

                    var filePath = string.Empty;
                    var balanceList = new List<BalanceSheetExelModels>();

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = "d:\\";
                    openFileDialog.Title = "Загрузить резульаты";
                    openFileDialog.Filter = "Файл Excel|*.xls;*.xlsx;*.xlsm";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.CheckPathExists = true;

                    if(openFileDialog.ShowDialog() == true)
                    {
                        filePath = openFileDialog.FileName;
                    }

                    if(filePath != string.Empty)
                    {

                        Workbook wb = new Workbook(filePath);


                        // Get all worksheets
                        WorksheetCollection collection = wb.Worksheets;

                        // Loop through all the worksheets
                        for (int worksheetIndex = 0; worksheetIndex < collection.Count; worksheetIndex++)
                        {
                            // Get worksheet using its index
                            Worksheet worksheet = collection[worksheetIndex];

                            // Print worksheet name
                            Console.WriteLine("Worksheet: " + worksheet.Name);

                            // Get number of rows and columns
                            int rows = worksheet.Cells.MaxDataRow;
                            int cols = worksheet.Cells.MaxDataColumn;

                            // Loop through rows
                            for (int i = 0; i < rows; i++)
                            {
                                try
                                {

                                    var currentRowBalance = new BalanceSheetExelModels();
                                    currentRowBalance.BalanceId = worksheet.Cells[i, 0].Value + "";
                                    currentRowBalance.OpeningBalanceAssets = Convert.ToInt64(worksheet.Cells[i, 1].Value);
                                    currentRowBalance.OpeningBalancePassive = Convert.ToInt64(worksheet.Cells[i, 2].Value);
                                    currentRowBalance.Debit = Convert.ToInt64(worksheet.Cells[i, 3].Value);
                                    currentRowBalance.Credit = Convert.ToInt64(worksheet.Cells[i, 4].Value);
                                    currentRowBalance.ClosingBalanceAssets = Convert.ToInt64(worksheet.Cells[i, 5].Value);
                                    currentRowBalance.ClosingBalancePassive = Convert.ToInt64(worksheet.Cells[i, 6].Value);

                                    balanceList.Add(currentRowBalance);
                                }
                                catch { continue; }


                                // Loop through each column in selected row
                                for (int j = 0; j <= cols; j++)
                                {
                                    // Pring cell value
                                    Text += worksheet.Cells[i, j].Value + " | ";
                                }
                                // Print line break
                                Text += "\n";
                            }
                            
                            var headerList = new List<string>();

                            balanceList = ParceFile(balanceList, ref headerList);

                            MessageBox.Show(balanceList.Count.ToString());
                            MessageBox.Show(headerList.Count.ToString());

                            Text = "";


                            var context = new DBContext();
                            bool isCreatingDB = context.Database.EnsureCreated();
                            if (isCreatingDB)
                            {
                                MessageBox.Show("База данных отсутствовала и была автоматически создана в процессе запуска программы.");
                            }

                            IBalanceSheets balanceSheets = new DatabaseRepository(context);

                            foreach(var balance in balanceList)
                            {
                                Text += balance + "\n";
                                balanceSheets.AddBalanceSheet(new BalanceSheetDB
                                {
                                    OpeningBalanceAssets = balance.OpeningBalanceAssets,
                                    OpeningBalancePassive = balance.OpeningBalancePassive,
                                    Debit = balance.Debit,
                                    Credit = balance.Credit,
                                    ClosingBalanceAssets = balance.ClosingBalanceAssets,
                                    ClosingBalancePassive = balance.ClosingBalancePassive,
                                });
                            }
                            balanceSheets.SaveChanges();

                            var result = balanceSheets.GetAllBalanceSheets;
                        }
                    }

                }));
            }
        }


        private List<BalanceSheetExelModels> ParceFile(List<BalanceSheetExelModels> list, ref List<string> headerList)
        {
            var resultList = new List<BalanceSheetExelModels>();
            int length = list.Count;
            for(int i = 0; i < length; ++i)
            {
                bool lineIsEmpty = (
                    list[i].OpeningBalanceAssets == 0 &&
                    list[i].OpeningBalancePassive == 0 &&
                    list[i].Debit == 0 &&
                    list[i].Credit == 0
                    );
                if(!lineIsEmpty)
                {
                    resultList.Add(list[i]);
                }
                else
                {
                    headerList.Add(list[i].BalanceId);
                }
            }
            return resultList;
        }

        /*
        public ICommand NAME
        {
            get
            {
                return new DelegateComand(obj => Task.Factory.StartNew(() =>
                {



                }));
            }
        }

        
            <DataGrid Height="400"
                      x:Name="dataGrid"
                      IsReadOnly="True"
                      FontSize="16">

            </DataGrid>

         */
    }
}
