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
        private SimPageProcessViewModel _processviewModel;
        private ProcessHelper _processHelper;
        private int _tabIndex;        
        public SimPageUserControl2(List<PacketModel> items, int tabIndex = -1)
        {
            _tabIndex = tabIndex;
            InitializeComponent();

            _processviewModel = new SimPageProcessViewModel(items); // initialize the _processviewModel instance                                 
            DataContext = _processviewModel;
            _processviewModel.PopulateRegionLists(items); // Getting the regions from viewmodel  

            _processHelper = new ProcessHelper(items, this, _processviewModel); // Pass a reference to this UserControl to the ProcessHelper constructor  

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
                    if (!_processviewModel.IsActive) // Check whether the control is active
                    {
                        break;
                    }
                    _processviewModel.IsIterationContinue = true;
                    IterationChecker();
                    if (_processviewModel.StopAfterIteration)
                    {
                        _processviewModel.StopAfterIteration = false;
                        _processviewModel.IsIterationContinue = false;
                        IterationChecker();
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
                        _processHelper.NormalProgressAsync();
                        _processviewModel.StopAfterIteration = false;
                        _processviewModel.IsIterationContinue = false;
                        IterationChecker();
                        break;
                    }
                }
            });
            thread.Start();
        }
        public void Message()
        {
            _processviewModel.result = MessageBox.Show(
            "Number of distincts all regions: " + _processviewModel.RegionList.Count + "\r\n" +
            "Number of distinct region1: " + _processviewModel.Region1List.Count + "\r\n" +
            "Number of distinct region2: " + _processviewModel.Region2List.Count + "\r\n" +
            "Number of distinct region3: " + _processviewModel.Region3List.Count + "\r\n" +
            "Number of distinct region4: " + _processviewModel.Region4List.Count + "\r\n" +
            "Number of distinct region5: " + _processviewModel.Region5List.Count + "\r\n" +
            "Driver name changed. New driver name: " + DriverNameTextBox.Text + "\r\n" +
            "Device number changed. New device number: " + comboBoxTestexa.Text,
            "Changes has been detected!", MessageBoxButton.OKCancel);
        }
        public void MessageAction()
        {
            DriverNameTextBox.IsEnabled = false;
            comboBoxTestexa.IsEnabled = false;

            if (_processviewModel.result == MessageBoxResult.OK)
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
                chkForwardLoop.IsChecked = false;
            }
            else if (checkBox == chkForwardLoop)
            {
                _processviewModel.ForwardloopEnabled = true;
                chkReverseLoop.IsChecked = false;
            }
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox == chkReverseLoop)
            {
                _processviewModel.ReverseloopEnabled = false;
                chkForwardLoop.IsChecked = false;
            }
            else if (checkBox == chkForwardLoop)
            {
                _processviewModel.ForwardloopEnabled = false;
                chkReverseLoop.IsChecked = false;
            }
        }
        private void comboBoxTestexa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTestexa.SelectedItem is ComboBoxItem comboBoxItem)
            {
                string selectedTestexa = (string)comboBoxItem.Content;
                string updatedPackageurl = WebRequestHelper.packageurl.Replace("{0}", selectedTestexa); // Replacing chosen device with placeholder
                _processviewModel.PackageUrl = updatedPackageurl;
                
                int currentTabIndex = (int)((TabItem)Parent).Tag; // Disable the selected item in other tabs
                foreach (var kvp in _processviewModel.SelectedComboItemsPerTab)
                {
                    int tabIndex = kvp.Key;
                    List<string> selectedItems = kvp.Value;
                    if (tabIndex != currentTabIndex && selectedItems.Contains(selectedTestexa))
                    {
                        comboBoxTestexa.SelectedItem = null;
                        comboBoxTestexa.IsEnabled = false;
                        comboBoxTestexa.IsEnabled = true;
                        MessageBox.Show("This device is already selected in another tab.", "Device already selected", MessageBoxButton.OK);
                        return;
                    }
                }                
                _processviewModel.SelectedComboItemsPerTab[currentTabIndex].Clear(); // Add the selected item to the list for the current tab
                _processviewModel.SelectedComboItemsPerTab[currentTabIndex].Add(selectedTestexa);
            }
            else if (comboBoxTestexa.SelectedItem is StackPanel stackPanel)
            {
                string selectedTestexa = (string)((TextBox)stackPanel.Children[1]).Text;
                string updatedPackageurl = WebRequestHelper.packageurl.Replace("{0}", selectedTestexa); // Replacing entered device with placeholder
                _processviewModel.PackageUrl = updatedPackageurl;
            }
        }
        private void IterationChecker()
        {
            if (_processviewModel.IsIterationContinue == true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow win = (MainWindow)Window.GetWindow(this); // Looking for the iteration status then coloring tab background as for the iteration status
                    if (win == null)
                    {
                        return;
                    }
                    win.UpdateTabColor(_tabIndex, true);
                });
            }
            else if (_processviewModel.IsIterationContinue == false)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow win = (MainWindow)Window.GetWindow(this);
                    if (win == null)
                    {
                        return;
                    }
                    win.UpdateTabColor(_tabIndex, false);
                });
            }
        }
    }
}