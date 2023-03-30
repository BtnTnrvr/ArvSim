using Sim2.Helper;
using Sim2.Models;
using Sim2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private MessageHelper _messageHelper;
        private int _tabIndex;
        public SimPageUserControl2(List<PacketModel> items, int tabIndex = -1)
        {
            _tabIndex = tabIndex;
            InitializeComponent();

            _messageHelper = new MessageHelper(items, this);

            _processviewModel = new SimPageProcessViewModel(items); // initialize the _processviewModel instance                                 
            DataContext = _processviewModel;

            _processHelper = new ProcessHelper(items, this, _processviewModel); // Pass a reference to this UserControl to the ProcessHelper constructor  

            btnContinue.IsEnabled = false; // Disable buttons on startup
            btnPause.IsEnabled = false;
            btnDateTimer.IsEnabled = false;

            DriverNameTextBox.TextChanged += Input_ValueChanged; // Is there an input in the textbox and combobox
            comboBoxTestexa.SelectionChanged += Input_ValueChanged;
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
                _messageHelper.Message();
                _messageHelper.MessageAction();
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
        private void Input_ValueChanged(object sender, EventArgs e)
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
        private void CheckBox_Checked(object sender, RoutedEventArgs e) // Handles the reverse and forward, forward loop checked values
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
                CompareSelectedItems(selectedTestexa);
            }
            else if (comboBoxTestexa.SelectedItem is StackPanel stackPanel)
            {
                string selectedTestexa = (string)((TextBox)stackPanel.Children[1]).Text;
                if (string.IsNullOrEmpty(selectedTestexa)) return;
                CompareSelectedItems(selectedTestexa);
            }
        }
        private void CheckSelectedItems(string selectedTestexa, int currentTabIndex)
        {
            foreach (var keyValuePair in _processviewModel.SelectedComboItemsPerTab)
            {
                int tabIndex = keyValuePair.Key;
                List<string> selectedItems = keyValuePair.Value;
                if (tabIndex != currentTabIndex && selectedItems.Contains(selectedTestexa))
                {
                    if (comboBoxTestexa.SelectedItem is ComboBoxItem)
                    {
                        comboBoxTestexa.SelectedItem = null;
                    }
                    else if (comboBoxTestexa.SelectedItem is StackPanel stackPanel)
                    {
                        ((TextBox)stackPanel.Children[1]).Clear();
                        comboBoxTestexa.SelectedItem = null;
                    }
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    TabItem tabItem = mainWindow.tabControl.Items.Cast<TabItem>().FirstOrDefault(item => (int)item.Tag == tabIndex);
                    string tabName = tabItem?.Header?.ToString() ?? $"Tab {tabIndex}";
                    MessageBox.Show($"This device is already selected in tab '{tabName}'.", "Device already selected", MessageBoxButton.OK);
                    return;
                }
            }
        }
        private void CompareSelectedItems(string selectedTestexa)
        {
            string updatedPackageurl = WebRequestHelper.packageurl.Replace("{0}", selectedTestexa);
            _processviewModel.PackageUrl = updatedPackageurl;
            int currentTabIndex = (int)((TabItem)Parent).Tag;
            CheckSelectedItems(selectedTestexa, currentTabIndex);
            _processviewModel.SelectedComboItemsPerTab[currentTabIndex].Clear();
            _processviewModel.SelectedComboItemsPerTab[currentTabIndex].Add(selectedTestexa);
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