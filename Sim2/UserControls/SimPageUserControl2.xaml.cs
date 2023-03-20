using Sim2.Helper;
using Sim2.Models;
using Sim2.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sim2.UserControls
{
    public partial class SimPageUserControl2 : UserControl
    {        
        private SimPageRegionViewModel regionviewModel;
        private SimPageProcessViewModel _processviewModel;
        private SimPageComboBoxViewModel _comboboxviewModel;
        private ProcessHelper _processHelper;
        private int _tabIndex;
        public SimPageUserControl2(List<PacketModel> items, int tabIndex = -1)
        {
            _tabIndex = tabIndex;
            InitializeComponent();
            _comboboxviewModel = new SimPageComboBoxViewModel();

            _processviewModel = new SimPageProcessViewModel(items); // initialize the _processviewModel instance                                 
            DataContext = _processviewModel; 

            _processHelper = new ProcessHelper(items, this, _comboboxviewModel); // Pass a reference to this UserControl to the ProcessHelper constructor  

            regionviewModel = new SimPageRegionViewModel();
            regionviewModel.PopulateRegionLists(items); // Getting the regions from viewmodel         

            btnContinue.IsEnabled = false; // Disable buttons on startup
            btnPause.IsEnabled = false;
            btnDateTimer.IsEnabled = false;

            DriverNameTextBox.TextChanged += Input_TextChanged; // Is there an input in the textbox and combobox
            comboBoxTestexa.SelectionChanged += Input_TextChanged;
        }
        public void ProcessItems()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    if (_processviewModel.StopAfterIteration)
                    {
                        _processviewModel.StopAfterIteration = false;
                        return;
                    }
                    if (_processviewModel.ReverseloopEnabled)
                    {
                        _processHelper.ReverseLoop();
                    }
                    else if (_processviewModel.ForwardloopEnabled)
                    {
                        _processHelper.ForwardLoop();
                    }
                    else
                    {
                        _processHelper.NormalProgress();
                        break;
                    }
                }
            });
            thread.Start();
        }
        public void Message()
        {
            regionviewModel.result = MessageBox.Show(
            "Number of distincts all regions: " + regionviewModel.RegionList.Count + "\r\n" +
            "Number of distinct region1: " + regionviewModel.Region1List.Count + "\r\n" +
            "Number of distinct region2: " + regionviewModel.Region2List.Count + "\r\n" +
            "Number of distinct region3: " + regionviewModel.Region3List.Count + "\r\n" +
            "Number of distinct region4: " + regionviewModel.Region4List.Count + "\r\n" +
            "Number of distinct region5: " + regionviewModel.Region5List.Count + "\r\n" +
            "Driver name changed. New driver name: " + DriverNameTextBox.Text + "\r\n" +
            "Device number changed. New device number: " + comboBoxTestexa.Text,
            "Changes has been detected!", MessageBoxButton.OKCancel);
        }
        public void MessageAction()
        {
            DriverNameTextBox.IsEnabled = false;
            comboBoxTestexa.IsEnabled = false;

            if (regionviewModel.result == MessageBoxResult.OK)
            {
                btnContinue.IsEnabled = false;
                btnPause.IsEnabled = true;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ProcessItems();
                }));
            }
            else
            {
                btnContinue.IsEnabled = true;
                btnPause.IsEnabled = false;
                DriverNameTextBox.IsEnabled = true;
                comboBoxTestexa.IsEnabled = true;
                return;
            }
        }
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = listViewData.SelectedItem as PacketModel;

            if (selectedItem == null)
            {
                return;
            }
            var editPage = new EditPage(selectedItem);

            if (editPage.ShowDialog() == true)
            {
                var editedItem = editPage.EditedItem;
                foreach (var property in typeof(PacketModel).GetProperties()) // Update each property of the selected item with the edited values.
                {
                    var editedValue = property.GetValue(editedItem);
                    if (editedValue != null)
                    {
                        property.SetValue(selectedItem, editedValue);
                    }
                }
                selectedItem.TimeDifferenceAsTimeSpan = editedItem.TimeDifferenceAsTimeSpan; // Update the TimeDifferenceAsTimeSpan property of the selected item with the edited value.
            }
            listViewData.Items.Refresh();
        }
        private void btnDateTimer_Click(object sender, RoutedEventArgs e)
        {
            comboBoxTestexa.IsEnabled = false;
            DriverNameTextBox.IsEnabled = false;
            btnDateTimer.IsEnabled = false;
            btnContinue.IsEnabled = false;
            _processviewModel.IsPaused = false;

            DateTime selectedDateTime = timePicker.Value ?? DateTime.MinValue;
            DispatcherTimer timer = new DispatcherTimer(); // Create a DispatcherTimer to check the current time
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                if (DateTime.Now >= selectedDateTime)
                {
                    timer.Stop();
                    btnDateTimer.IsEnabled = true;
                    ProcessItems();
                    btnPause.IsEnabled = true;
                }
            };
            timer.Start();
        }
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            btnPause.IsEnabled = true;

            if (_processviewModel.HasShownMessageBox)
            {
                _processviewModel.HasShownMessageBox = false;
                ProcessItems();
            }
            else
            {
                Message();
                MessageAction();
            }
            _processviewModel.IsPaused = false;
            if (btnPause.IsEnabled == true)
            {
                btnContinue.IsEnabled = false;
            }
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.UpdateTabColor(_tabIndex, true);
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            _processviewModel.HasShownMessageBox = true;
            btnContinue.IsEnabled = true;

            _processviewModel.IsPaused = !_processviewModel.IsPaused;
            if (_processviewModel.IsPaused)
            {
                _processviewModel.StopAfterIteration = true;
            }
            if (btnContinue.IsEnabled == true)
            {
                btnPause.IsEnabled = false;
            }
            MainWindow win = (MainWindow)Window.GetWindow(this);
            win.UpdateTabColor(_tabIndex, false);
        }
        private void Input_TextChanged(object sender, EventArgs e)
        {
            if (DriverNameTextBox.Text.Trim().Length > 0 && comboBoxTestexa.SelectedItem != null)
            {
                btnContinue.IsEnabled = true;
                btnDateTimer.IsEnabled = true;
            }
            else
            {
                btnContinue.IsEnabled = false;
                btnDateTimer.IsEnabled = false;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e) // Handles the Reverse and Forward loop checked values
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox == chkReverseLoop)
            {
                _processviewModel.ReverseloopEnabled = true;
                chkForwardLoop.IsEnabled = false;
            }
            else if (checkBox == chkForwardLoop)
            {
                _processviewModel.ForwardloopEnabled = true;
                chkReverseLoop.IsEnabled = false;
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) // Handles the Reverse and Forward loop unchecked values
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox == chkReverseLoop)
            {
                _processviewModel.ReverseloopEnabled = false;
                chkForwardLoop.IsEnabled = true;
            }
            else if (checkBox == chkForwardLoop)
            {
                _processviewModel.ForwardloopEnabled = false;
                chkReverseLoop.IsEnabled = true;
            }
        }
        private void comboBoxTestexa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTestexa.SelectedItem is ComboBoxItem comboBoxItem)
            {
                string selectedTestexa = (string)comboBoxItem.Content;
                _comboboxviewModel.ChosenTestexas.Add(selectedTestexa);
                comboBoxItem.IsEnabled = false;
                string updatedPackageurl = WebRequestHelper.packageurl.Replace("{0}", selectedTestexa); // Replacing chosen device with placeholder
                _comboboxviewModel.PackageUrl = updatedPackageurl;
            }
            else if (comboBoxTestexa.SelectedItem is StackPanel stackPanel)
            {
                string selectedTestexa = (string)((TextBox)stackPanel.Children[1]).Text;
                string updatedPackageurl = WebRequestHelper.packageurl.Replace("{0}", selectedTestexa); // Replacing entered device with placeholder
                _comboboxviewModel.PackageUrl = updatedPackageurl;
            }
        }
    }
}