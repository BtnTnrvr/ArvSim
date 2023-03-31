using System;
using System.Collections.Generic;
using System.Windows;
using Sim2.ViewModels;
using Sim2.Models;
using Sim2.UserControls;
using System.Windows.Controls;

namespace Sim2.Helper
{
    internal class MessageHelper
    {
        private SimPageProcessViewModel _processviewModel;
        private SimPageUserControl2 _userControl;
        public MessageHelper(List<PacketModel> items, SimPageUserControl2 userControl)
        {
            _userControl = userControl;
            _processviewModel = new SimPageProcessViewModel(items); // initialize the _processviewModel instance   
            _processviewModel.PopulateRegionLists(items); // Getting the regions from viewmodel
        }
        public void Message()
        {
            string selectedText = ((_userControl.comboBoxTestexa.SelectedItem as ComboBoxItem)?.Content ?? "").ToString(); // For printing selected devices
            string typedText = (_userControl.comboBoxTestexa.SelectedItem as StackPanel)?.Children[1] is TextBox textBox ? textBox.Text : ""; // For printing typed devices 
            string displayedText = selectedText + typedText;
            _processviewModel.result = MessageBox.Show(
            "Number of distincts all regions: " + _processviewModel.RegionList.Count + "\r\n" +
            "Number of distinct region1: " + _processviewModel.Region1List.Count + "\r\n" +
            "Number of distinct region2: " + _processviewModel.Region2List.Count + "\r\n" +
            "Number of distinct region3: " + _processviewModel.Region3List.Count + "\r\n" +
            "Number of distinct region4: " + _processviewModel.Region4List.Count + "\r\n" +
            "Number of distinct region5: " + _processviewModel.Region5List.Count + "\r\n" +
            "Driver name changed. New driver name: " + _userControl.DriverNameTextBox.Text + "\r\n" +
            "Device number changed. New device number: " + displayedText,
            "Changes has been detected!", MessageBoxButton.OKCancel);
        }
        public void MessageAction()
        {
            _userControl.DriverNameTextBox.IsEnabled = false;
            _userControl.comboBoxTestexa.IsEnabled = false;

            if (_processviewModel.result == MessageBoxResult.OK)
            {
                _userControl.btnContinueDateTimer.IsEnabled = false;
                _userControl.btnPause.IsEnabled = true;

                _userControl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _userControl.ProcessItems();
                }));
            }
            else
            {
                _userControl.btnContinueDateTimer.IsEnabled = true;
                _userControl.btnPause.IsEnabled = false;
                _userControl.DriverNameTextBox.IsEnabled = true;
                _userControl.comboBoxTestexa.IsEnabled = true;
                return;
            }
        }
    }
}