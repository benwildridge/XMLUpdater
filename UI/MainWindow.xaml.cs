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
using System.ComponentModel;

// parrallel for each to increase performance
// add current board code and to board code on the logging

namespace UI
{

    public partial class MainWindow : Window
    {

        private string currentBoardCode = "";
        private string newBoardCode = "";
        private bool errorOccured = false;
        private bool fileUpdated = false;
        private int fileCount;
        private int fileUpdatedCount;
        private int fileProcessingCount;

        public static void VerifyDir(string path)
        //Method to check if the directory exists, if it doesn't it creates it. Used in the logging Method
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
            // Checks which checkboxes were ticked and compiles them into a List
            List<string> files = new List<string>();

            if ((bool)checkBoxSTD.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["STD"], "*.xml").ToList());
            }
            if ((bool)checkBoxWest.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["WEST"], "*.xml").ToList());
            }
            if ((bool)checkBoxMultiW.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["MULTIW"], "*.xml").ToList());
            }
            if ((bool)checkBoxMultIE.IsChecked)
            {
                files.AddRange(System.IO.Directory.GetFiles(System.Configuration.ConfigurationManager.AppSettings["MULTIE"], "*.xml").ToList());
            }
            fileCount = files.Count;
            fileUpdatedCount = 0;
            fileProcessingCount = 0;
            //foreach (string file in files)
            Parallel.ForEach(files, (file) =>
            {
                // For all the files in all the directories in the list, it loads the file, checks the BrdCode against the currentBoardCode and updates it if it matches. 
                //Then adds an entry to the log file, saves the file and flags the variable to show an update has taken place.
                //Error catch in place for if the XML document isn't formatted to have "//CutList/Board//BrdCode"
                try
                {
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(file);
                        fileProcessingCount = fileProcessingCount + 1;
                        XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                        string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                        if (XMLCurrentBoardCode == currentBoardCode)
                        {
                            root.SetAttribute("BrdCode", $"{newBoardCode}");
                            Logger($"{file} " + "was updated");
                            doc.Save(file);
                            fileUpdated = true;
                            fileUpdatedCount = fileUpdatedCount + 1;
                        }
                    }
                }
                catch (Exception)
                {
                    Logger($"{file} " + "could not be checked due to the XML formatting, please correct this before trying again.");
                    errorOccured = true;
                }
            });
        }

        public static void Logger(string lines)
        {
            //Logfile functionality method
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

        private void ButtonEnableChecks()
        // Checks that the inut textboxes have changed and that there is at least 1 tickbox selected in order to the do the update.
        {
            if (string.Equals(currentBoardCodeTextBox.Text, "BOD"))
            {
                updateButton.IsEnabled = false;
            }
            else if (string.Equals(currentBoardCodeTextBox.Text, "BOD"))
            {
                updateButton.IsEnabled = false;
            }
            else if (checkBoxSTD.IsChecked == true)
            {
                updateButton.IsEnabled = true;
            }
            else if (checkBoxWest.IsChecked == true)
            {
                updateButton.IsEnabled = true;
            }
            else if (checkBoxMultIE.IsChecked == true)
            {
                updateButton.IsEnabled = true;
            }
            else if (checkBoxMultiW.IsChecked == true)
            {
                updateButton.IsEnabled = true;
            }
            else
            {
                updateButton.IsEnabled = false;
            }
        }

        private void checkBoxSTD_Checked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxSTD_UnChecked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxWest_Checked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxWest_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxMultiW_Checked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxMultiW_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxMultIE_Checked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }
        private void checkBoxMultIE_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonEnableChecks();
        }

        public MainWindow()
        {
            InitializeComponent();
            currentBoardCodeTextBox.Text = "BOD";
            newBoardCodeTextBox.Text = "BOD";
            updateButton.IsEnabled = false;
        }

        public void currentBoardCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonEnableChecks();
        }

        public void newBoardCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonEnableChecks();
        }


        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            //Produces message on screen to confirm that update will take place with selected paramamters. Yes proceeds up with update. No closes the process.

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
                //runs the through the XMLUpdate method and then brings back different messages based on what happen
                //No update - Update - Update with errors
                case MessageBoxResult.Yes:
                    currentBoardCode = currentBoardCodeTextBox.Text;
                    newBoardCode = newBoardCodeTextBox.Text;
                    XMLUpdate();
                    if (fileUpdated == false)
                    {
                        Logger("No files that could be checked met the criteria of updating" + $" {currentBoardCode} " + "to" + $" {newBoardCode} ");
                        MessageBox.Show("No files were updated. Please check C:\\Temp\\Log for details." + Environment.NewLine + "You can now close the application.");
                        Logger("Number of files checked: " + $"{fileCount}");
                        Logger("Number of files updated: " + $"{fileUpdatedCount}");
                        break;

                    }
                    else if (errorOccured == true)
                    {
                        MessageBox.Show("Update Complete." + Environment.NewLine + "Some files had errors. Please check C:\\Temp\\Log for details." + Environment.NewLine + "You can now close the application.");
                        Logger("Number of files checked: " + $"{fileCount}");
                        Logger("Number of files updated: " + $"{fileUpdatedCount}");
                        break;

                    }

                    else
                    {
                        //TODO - Update Message Box to show nicer
                        MessageBox.Show("Update Complete. Check C:\\Temp\\Log for details of files updated." + Environment.NewLine + "You can now close the application.");
                        Logger("Number of files checked: " + $"{fileCount}");
                        Logger("Number of files updated: " + $"{fileUpdatedCount}");
                        break;

                    }
                case MessageBoxResult.No:
                    break;
            }

        }

    }
}
