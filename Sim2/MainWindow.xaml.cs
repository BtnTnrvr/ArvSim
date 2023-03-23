using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using Sim2.Models;
using Sim2.UserControls;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using Sim2.Helper;
using System.Windows.Media;
using Sim2.ViewModels;

namespace Sim2
{
    public partial class MainWindow : Window
    {
        private int globalIndex = 0;
        private SimPageProcessViewModel _processviewModel;
        public MainWindow()
        {
            InitializeComponent();
            tabControl.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }
        public List<PacketModel> ProcessData(string fileContents)
        {
            var allModel = CalculateDeserializedHelper.GetAllModelFromFile(fileContents);
            CalculateDeserializedHelper.CalculateTimeDifference(allModel);
            return allModel;
        }
        private void File2ReadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var allModel = ProcessData(File.ReadAllText(filePath));

                if (tabControl.Items.Cast<TabItem>().Any(tab => (int)tab.Tag == globalIndex))
                {
                    MessageBoxResult result = MessageBox.Show("This file is already open. Do you want to open it again?", "File already open", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        AddNewTab(filePath, allModel);
                    }
                }
                else
                {
                    AddNewTab(filePath, allModel);
                }
                CalculateDeserializedHelper.CalculateIncrementalSizes(allModel);
            }
        }
        private void AddNewTab(string filePath, List<PacketModel> itemList)
        {
            TabItem newTab = new TabItem();
            newTab.Header = Path.GetFileName(filePath);
            newTab.Tag = globalIndex;
            newTab.Content = new SimPageUserControl2(itemList, globalIndex);
            globalIndex++;
            tabControl.Items.Add(newTab);            
        }
        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (tabControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {                
                foreach (var tab in tabControl.Items.OfType<TabItem>()) // For each TabItem in the tabControl's Items collection, set up a mouse double-click event handler and create a context menu.
                {
                    tab.MouseDoubleClick += TabItem_MouseDoubleClick;
                    tab.ContextMenu = TabDelete(tab);
                }
            }
        }
        private ContextMenu TabDelete(TabItem tabItem)
        {
            var menu = new ContextMenu();
            var deleteItem = new MenuItem { Header = "Delete" };

            deleteItem.Click += (sender, e) => // Set up a click event handler for the delete MenuItem that removes the TabItem from the tabControl's Items collection.
            {
                _processviewModel = new SimPageProcessViewModel();
                _processviewModel.IsActive = false; // IsActive property that will stopped the iteration after delete
                tabControl.Items.Remove(tabItem);
            };
            menu.Items.Add(deleteItem);
            return menu;
        }
        private void TabItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tabItem = sender as TabItem;

            if (tabItem == null) return;
            if (tabItem.Header is TextBox) return;

            TextBox textBox = new TextBox();
            textBox.Text = tabItem.Header.ToString();
            textBox.PreviewKeyDown += TextBox_PreviewKeyDown;            
            tabItem.Header = textBox; // Replace the TabItem header with the TextBox
            textBox.Focus();
            textBox.SelectAll();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                TabItem tabItem = textBox.Parent as TabItem;
                
                tabItem.Header = textBox.Text.Trim() == "" ? "New Tab" : textBox.Text; // Restore the TabItem header to a TextBlock
                tabItem.HeaderTemplate = null;
                
                e.Handled = true; // Prevent the TextBox from capturing the Enter key
            }
        }
        public void UpdateTabColor(int tabIndex, bool v)
        {
            foreach( var item in (this.tabControl.Items)) 
            {
                if (v && ((TabItem)item).Tag.ToString().Equals(tabIndex.ToString())) 
                {
                    ((TabItem)item).Background = Brushes.LightGreen;
                }
                else if (!v && ((TabItem)item).Tag.ToString().Equals(tabIndex.ToString()))
                {
                    ((TabItem)item).Background = Brushes.Red;
                }
            }
            var tabControlList = this.tabControl.Items;
        }
    }
}