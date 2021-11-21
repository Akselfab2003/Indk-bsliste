using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using Brushes = System.Drawing.Brushes;
using Case_Opgave_på_USB;
using Newtonsoft.Json.Linq;

namespace Case_Opgave_på_USB
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Font verdana10Font;
        private StreamReader reader;
        public Window1(JObject jobj,string Name)
        {
            InitializeComponent();
            ChangeAfdelingLabel(Name);
            InsertValuesToVareList(jobj,Name);
            string NameString = Name;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //Lavet med hjælp fra denne vejledning  https://www.c-sharpcorner.com/article/printing-text-file-in-C-Sharp/ ;
        private void printButton_Click(object sender, RoutedEventArgs e)
        { string Name = Convert.ToString(AfdelingLabel.Content.ToString());
            string FileName = (@"E:\\Case Indkøbsliste\\"+Name+".txt");
            reader = new StreamReader(FileName);
            verdana10Font = new Font("Verdana",10);
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
            pd.Print();
            if (reader != null)
                reader.Close();
        }
        private void PrintTextFileHandler(object sender, PrintPageEventArgs ppeArgs)
        {
            
            Graphics g = ppeArgs.Graphics;
            float linesPerPage = 0;
            float yPos = 0;
            int Count = 0;
            float leftMargin = ppeArgs.MarginBounds.Left;
           float topMargin = ppeArgs.MarginBounds.Top;
            string line = null;
            linesPerPage = ppeArgs.MarginBounds.Height / verdana10Font.GetHeight (g);
            while (Count < linesPerPage && ((line = reader.ReadLine()) != null))
                {
                yPos = topMargin + (Count * verdana10Font.GetHeight(g));
                g.DrawString(line, verdana10Font, Brushes.Black, leftMargin, yPos, new StringFormat());
                Count++;
                }
            if(line != null)
            {
                ppeArgs.HasMorePages = true;
            }
            else
            {
                ppeArgs.HasMorePages = false;
            }
        }
        //Indsætter Værdier i VareList
        public void InsertValuesToVareList(JObject jobj,string Name)
        {
         for(int i = 0; i < jobj[Name].Count(); i++)
            {
               VareListView.Items.Add(jobj[Name][i]);

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        //Ændre Navnet På Afdelingen
        public void ChangeAfdelingLabel(string Name)
        {
            AfdelingLabel.Content = Name;
        }
    }
}
