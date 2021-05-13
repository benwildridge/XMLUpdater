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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //TODO - Update window to be Howdenised
    //TODO - Block update button if no tickboxes are checked
    //TODO - Add error handling if the XML is not in the correct structure and output to log file.


    public partial class MainWindow : Window
    {

        private string currentBoardCode = "";
        private string newBoardCode = "";


        public static void VerifyDir(string path)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch { }
        }

        public void XMLUpdate()
        {

            List<string> files = new List<string>();

            if ((bool)checkBoxSTD.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["STD"], "*.xml").ToList());
            }
            if((bool)checkBoxWest.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["WEST"], "*.xml").ToList());
            }
            if((bool)checkBoxMultiW.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["MULTIW"], "*.xml").ToList());
            }
            if ((bool)checkBoxMultIE.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["MULTIE"], "*.xml").ToList());
            }
                foreach (string file in files)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                if (XMLCurrentBoardCode == currentBoardCode)
                {
                    root.SetAttribute("BrdCode", $"{newBoardCode}");
                    Logger($"{file} " + "was updated");
                    doc.Save(file);
                }

            }
        }

        public static void Logger(string lines)
        {
            string path = "C:/temp/Log/";
            VerifyDir(path);
            string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_Logs.txt";
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
                file.WriteLine(DateTime.Now.ToString() + ": " + lines);
                file.Close();
            }
            catch (Exception) { }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void currentBoardCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        public void newBoardCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            currentBoardCode = currentBoardCodeTextBox.Text;
            newBoardCode = newBoardCodeTextBox.Text;
            var Checkboxlist = $"The following folders will be updated: "
                + Environment.NewLine + ""
                + Environment.NewLine +
                ((bool)checkBoxSTD.IsChecked ? "STD " : "") +
                ((bool)checkBoxWest.IsChecked ? "WEST " : "") +
                ((bool)checkBoxMultIE.IsChecked ? "MULTIE " : "") +
                ((bool)checkBoxMultiW.IsChecked ? "MULTIW " : "")
                + Environment.NewLine + ""
                + Environment.NewLine + "With the below values:"
                + Environment.NewLine + ""
                + Environment.NewLine + "Current Board Code: " + $"{currentBoardCode}"
                + Environment.NewLine + "New Board Code: " + $"{newBoardCode}";

            string sCaption = "XMLUpdater";
            


            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(Checkboxlist, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    currentBoardCode = currentBoardCodeTextBox.Text;
                    newBoardCode = newBoardCodeTextBox.Text;
                    XMLUpdate();
                    //TODO - Update Message Box to show nicer
                    MessageBox.Show("Update Complete. Check C:\\Temp\\Log for details of files updated." + Environment.NewLine + "You can now close the application.");
                    break;

                case MessageBoxResult.No:
                    break;
            }

        }
    }
}
