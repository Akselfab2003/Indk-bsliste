using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Case_Opgave_på_USB;
using Json.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Indkøbs_list_applikation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InsertcategoriesAndDepartments();
            GetJson();
            
            
        }
        //Læser en Json fil
        public JObject GetJson()
        {
            StreamReader file = new StreamReader($"E:\\Case Indkøbsliste\\file.JSON");
            string result = string.Empty;
            var json = file.ReadToEnd();
            var jobj = JObject.Parse(json);
             var Kategorier = jobj["Kategorier"];
              string[] KategorierArray = new string[3];
              string[] departments = { "Smede værksted", "Administration(kontor)", "Service afdeling" };
              for (int i = 0; i < 3; i++)
              {
                  KategorierArray[i] = Kategorier[i].ToString();
               
              }
              
              for (int FillShopinglist = 0; FillShopinglist < 2; FillShopinglist++)
              {
                  VareListe.Items.Add(jobj["Vare"][KategorierArray[0]][FillShopinglist]);
                  VareListe.Items.Add(jobj["Vare"][KategorierArray[1]][FillShopinglist]);
                  VareListe.Items.Add(jobj["Vare"][KategorierArray[2]][FillShopinglist]);
              }
            VareListe.Items.Clear();

             
            return jobj;
        }
        //Indsætter Kategorierne i VælgVareKatagori og Afdelinger
        public void InsertcategoriesAndDepartments()
        {
            StreamReader file = new StreamReader($"E:\\Case Indkøbsliste\\file.JSON");
            string result = string.Empty;
            var json = file.ReadToEnd();
            var jobj = JObject.Parse(json);
            var Kategorier = jobj["Kategorier"];
            string[] KategorierArray = new string[3];
            string[] departments = { "Smede værksted", "Administration(kontor)", "Service afdeling" };
            for (int i = 0; i < 3; i++)
            {
                KategorierArray[i] = Kategorier[i].ToString();
                VælgVareKatagori.Items.Add(Kategorier[i].ToString());
                Afdelinger.Items.Add(departments[i]);

            }

        }

        //Indsætter Varerne i en notet fil så jeg ved hvad for nogen vare de har valgt
        public void AddwordToNote(string Word)
        {
            StreamReader reader = new StreamReader(@"E:\\Case Indkøbsliste\\Var_=_int.string_=_true.txt");
            string[]ArrayT = reader.ReadToEnd().Split(':');
            reader.Close();
            StreamWriter writer = new StreamWriter(@"E:\\Case Indkøbsliste\\Var_=_int.string_=_true.txt");
            ArrayT[ArrayT.Length - 1] = Word;
            for (int i = 0; i < ArrayT.Length; i++)
            {      if(i == ArrayT.Length)
                {
                    writer.Write(ArrayT[i]);
                 
                }
                else
                {
                    writer.Write(ArrayT[i]+ ":");
                }
                  
            }

            writer.Close();
   

        }
        //Læser jeg notet filen og Fjerner noget som ikke skal være der og Derefter Splitter jeg text på :
        public string[] ReadFromNote()
        {
            StreamReader reader = new StreamReader(@"E:\\Case Indkøbsliste\\Var_=_int.string_=_true.txt");
            string Textline = reader.ReadLine();
            string[] ArrayOfClickWordsFromVareList = Textline.Remove(Textline.Length-1).Split(':');
            reader.Close();
            StreamWriter writer = new StreamWriter(@"E:\\Case Indkøbsliste\\Var_=_int.string_=_true.txt");
            writer.WriteLine("");
            writer.Close();
            return ArrayOfClickWordsFromVareList;
        }
       
        
        public static class GlobalData
        {

            public static int NumberofItemsInIndkøbsliste;
            public static string[] IndkøbslisteArrayOfItems = new string[GlobalData.NumberofItemsInIndkøbsliste];
            public static string Smede_værkstedJsonFormatList;
            public static string administrationJsonFormatList;
            public static string service_afdelingJsonFormatList;

        }

        //Her Skriver Jeg Indkøbs selden Til afdelingen og indsætter navn og dato Samt tilføjer Afdelings til Indkøbslister
        public void WriteToJson(string name,string value)
        { DateTime now = DateTime.Now;
            string NOW = now.ToString();
            string path = (@"E:\\Case Indkøbsliste\\"+name+".txt");
            
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine("Navn:{0} Dato/tid:{1}",name,NOW);
            writer.WriteLine(value);
            writer.Close();
            Indkøbslister.Items.Add(name);
        }

        

        private void VareListe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }

        //Tjekker hvad der er blevet klikket på i Vareliste og Tilføjer hvad der blev klikket på til Indkøbsliste og ændre baggrundfarven på det item der er klikket på til rød
        private void DetectMousepress(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                if (Indkøbsliste.Items.Contains(VareListe.SelectedItem))
                {

                }
                else
               {
                    Indkøbsliste.Items.Add(VareListe.SelectedItem);
                    AddwordToNote(VareListe.SelectedItem.ToString());
                    ListViewItem Item1 = sender as ListViewItem;
                    Item1.Background = Brushes.Red;
                    }
            }

        }
        //Gemmer IndkøbsListen i en Json format samt clear Indkøbsliste og Vareliste
        private void GemListe_Click(object sender, RoutedEventArgs e)
        {
           string[] ArrayOfClickWordsFromVareList = ReadFromNote();
           
            string Test = "";
            if (Afdelinger.SelectedItem.ToString() == "Smede værksted")
            {
                
               Test = "{\"Smede værksted\":[";
                for (int i = 0; i < ArrayOfClickWordsFromVareList.Length; i++)
                {
                    Test = Test + ("\""+ArrayOfClickWordsFromVareList[i]+"\"");
                    if (i != ArrayOfClickWordsFromVareList.Length-1)
                    {
                        Test  = Test + (",");
                        
                    }
                    

                }
                Test += "]}";
                WriteToJson("Smede værksted", Test);
             
                
            }
            else if (Afdelinger.SelectedItem.ToString() == "Administration(kontor)")
            {
                Test = "{\"Administration(kontor)\":[";
                for (int i = 0; i < ArrayOfClickWordsFromVareList.Length; i++)
                {
                    Test = Test + ("\"" + ArrayOfClickWordsFromVareList[i] + "\"");
                    if (i != ArrayOfClickWordsFromVareList.Length-1)
                    {
                        Test = Test + (",");

                    }


                }
                Test += "]}";
                WriteToJson("Administration(kontor)", Test);

            }
            else if (Afdelinger.SelectedItem.ToString() == "Service afdeling")
            {
                Test = "{\"Service afdeling\":[";
                for (int i = 0; i < ArrayOfClickWordsFromVareList.Length; i++)
                {
                    Test = Test + ("\"" + ArrayOfClickWordsFromVareList[i] + "\"");
                    if (i != ArrayOfClickWordsFromVareList.Length-1)
                    {
                        Test = Test + (",");

                    }


                }
                Test += "]}";
                WriteToJson("Service afdeling", Test);

            }
            Indkøbsliste.Items.Clear();
            VareListe.Items.Clear();
            
            
        }

        private void Afdelinger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        //Indsætter Varerne i VareListe når man vælger en kategori fra VælgVareKatagori
        public void InsertItems(JObject jobj)
        {
            var Kategorier = jobj["Kategorier"];
            string[] KategorierArray = new string[3];
            string[] departments = { "Smede værksted", "Administration(kontor)", "Service afdeling" };
            for (int i = 0; i < 3; i++)
            {
                KategorierArray[i] = Kategorier[i].ToString();
            }
            
            for (int FillShopinglist = 0; FillShopinglist < 2; FillShopinglist++)
            {
                if (VælgVareKatagori.SelectedItem.ToString() == "Madvare")
                {
                  
                    VareListe.Items.Add(jobj["Vare"][KategorierArray[0]][FillShopinglist]);
                   

                }
               else if (VælgVareKatagori.SelectedItem.ToString() == "Kontor Redskaber")
                {

             
                    VareListe.Items.Add(jobj["Vare"][KategorierArray[1]][FillShopinglist]);
                  

                }
                else if (VælgVareKatagori.SelectedItem.ToString() == "Sikkerheds Udstyr")
                {


                    VareListe.Items.Add(jobj["Vare"][KategorierArray[2]][FillShopinglist]);


                }
            }
        }
        //Køre når der bliver valgt en Kategori fra VælgVareKategori
        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VareListe.Items.Clear();
            JObject Json =GetJson();
            InsertItems(Json);

        }
        //Åbner Window1 når man klikker på en af Indkøbslisterne fra Indkøbslister som giver mulighed for at udprinte indkøbssedlen fra afdelingen
        private void DetectMousepress2(object sender, MouseButtonEventArgs e)
        {   
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
             string nameOfDepartment = Convert.ToString(Indkøbslister.SelectedItem.ToString());
                StreamReader file = new StreamReader($"E:\\Case Indkøbsliste\\"+nameOfDepartment+".txt");
                string result = string.Empty;
                file.ReadLine();
                var json = file.ReadToEnd();
                var jobj = JObject.Parse(json);
                object smed = jobj["Smede værksted"];
                string Name;
                if (Indkøbslister.SelectedItem.ToString() == "Smede værksted")
                {
                    Name = "Smede værksted";
                     Window1 window1 = new Window1(jobj,Name);
                     window1.Show();
                   
                }
               else if (Indkøbslister.SelectedItem.ToString() == "Administration(kontor)")
                {
                    Name = "Administration(kontor)";
                    Window1 window1 = new Window1(jobj, Name);
                    window1.Show();
                }
                else if (Indkøbslister.SelectedItem.ToString() == "Service afdeling")
                {
                    Name = "Service afdeling";
                    Window1 window1 = new Window1(jobj, Name);
                    window1.Show();
                }

            }
                
        }
    }
}
