﻿using System;
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
    public partial class MainWindow : Window
    {
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

        private string currentBoardCode = "";
        private string newBoardCode = "";

        
        //TODO File Change Logic Here
        public void UpdateFiles()
        {
            if ((bool)checkBoxSTD.IsChecked)
            {
                string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["STD"];

                var files = System.IO.Directory.GetFiles(xmlFilePath, "*.xml");
                foreach (string file in files)
                {
                   XmlDocument doc = new XmlDocument();
                   doc.Load(file);
                   XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                   string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                    if (XMLCurrentBoardCode == currentBoardCode)
                        {
                       root.SetAttribute("BrdCode", $"{newBoardCode}");
                        }

                    doc.Save(file);
                    Logger($"{file} " +  "was updated");
                }
            }
            if ((bool)checkBoxWest.IsChecked)
            {
                string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["WEST"];

                var files = System.IO.Directory.GetFiles(xmlFilePath, "*.xml");
                foreach (string file in files)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                    string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                    if (XMLCurrentBoardCode == currentBoardCode)
                    {
                        root.SetAttribute("BrdCode", $"{newBoardCode}");
                    }

                    doc.Save(file);
                    Logger($"{file} " + "was updated");
                }
            }
            if ((bool)checkBoxMultIE.IsChecked)
            {
                string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["MULTIE"];

                var files = System.IO.Directory.GetFiles(xmlFilePath, "*.xml");
                foreach (string file in files)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                    string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                    if (XMLCurrentBoardCode == currentBoardCode)
                    {
                        root.SetAttribute("BrdCode", $"{newBoardCode}");
                    }

                    doc.Save(file);
                    Logger($"{file} " + "was updated");
                }
            }
            if ((bool)checkBoxMultiW.IsChecked)
            {
                string xmlFilePath = System.Configuration.ConfigurationManager.AppSettings["MULTIW"];

                var files = System.IO.Directory.GetFiles(xmlFilePath, "*.xml");
                foreach (string file in files)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlElement root = (XmlElement)doc.DocumentElement.SelectSingleNode("//CutList/Board");
                    string XMLCurrentBoardCode = root.GetAttribute("BrdCode");

                    if (XMLCurrentBoardCode == currentBoardCode)
                    {
                        root.SetAttribute("BrdCode", $"{newBoardCode}");
                    }

                    doc.Save(file);
                    Logger($"{file} " + "was updated");
                }
            }
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
                    UpdateFiles();
                    break;

                case MessageBoxResult.No:
                    break;
            }

        }
    }
}