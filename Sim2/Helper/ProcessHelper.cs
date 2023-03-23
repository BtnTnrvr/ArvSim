using Sim2.Models;
using Sim2.UserControls;
using Sim2.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sim2.Helper
{
    public class ProcessHelper
    {
        private SimPageProcessViewModel _processviewModel;
        private SimPageComboBoxViewModel _comboboxviewModel;
        private SimPageUserControl2 _simPageUserControl2;
        public ProcessHelper(List<PacketModel> items, SimPageUserControl2 simPageUserControl2, SimPageComboBoxViewModel comboBoxViewModel)
        {
            _simPageUserControl2 = simPageUserControl2;
            _processviewModel = new SimPageProcessViewModel(items);
            _simPageUserControl2.listViewData.ItemsSource = _processviewModel.DisplayedDataList; 
            _comboboxviewModel = comboBoxViewModel;
        }        
        public async Task ReverseLoop()
        {            
            for (int i = 0; i < _processviewModel.DisplayedDataList.Count; i++)
            {
                await UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                TimerLoop();
            }
            ResetBackgroundListView();

            for (int i = _processviewModel.DisplayedDataList.Count - 1; i >= -1; i--)
            {
                await UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                TimerLoop();
            }
            ResetBackgroundListView();

            if (_processviewModel.StopAfterIteration == true || _processviewModel.DisplayedDataList.Count == 1) // Checking for the iteration comes to the end in reverse loop if its, brushing it green so iteration could stop in a order
            {
                _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].BackgroundColor = Brushes.LightGreen;
            }
            await UpdateBackgroundListViewAsync();
            ResetBackgroundListView();
        }
        public async Task ForwardLoop()
        {
            for (int i = 0; i < _processviewModel.DisplayedDataList.Count; i++)
            {
                await UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                TimerLoop();
            }
            ResetBackgroundListView();

            if (_processviewModel.CurrentIndex == _processviewModel.DisplayedDataList.Count) // Check if the end of the list has been reached and reset the index
            {
                _processviewModel.CurrentIndex = 0;
            }
        }
        public async Task NormalProgressAsync()
        {
            for (int i = 0; i < _processviewModel.DisplayedDataList.Count; i++)
            {
                await UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                TimerLoop();
            }
            await UpdateBackgroundListViewAsync();
        }
        private void ResetBackgroundListView()
        {
            foreach (var data in _processviewModel.DisplayedDataList) // Reset the background color and status text of all the rows in the displayed data list
            {
                data.BackgroundColor = Brushes.White;
                data.Status = string.Empty;
            }
        }
        public void TimerLoop()
        {
            var timer = new Stopwatch();

            if (_processviewModel.CurrentIndex < _processviewModel.DisplayedDataList.Count)
            {
                timer.Restart();
                while (timer.Elapsed < _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].TimeDifferenceAsTimeSpan) // Loop until the time specified by the current item's TimeDifferenceAsTimeSpan property has passed
                {
                    Thread.Sleep(100);
                }
            }
        }
        private void MoveToTheNextLine()
        {
            if (_processviewModel.CurrentIndex + 1 < _processviewModel.DisplayedDataList.Count) // Update the background color of the next row of data to yellow and increment the current index
            {
                _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex + 1].BackgroundColor = Brushes.Yellow;
                _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex + 1].Status = "Sending...";
            }
            _processviewModel.CurrentIndex++;
        }
        private async Task UpdateBackgroundListViewAsync()
        {
            if (_processviewModel.CurrentIndex < 0 || _processviewModel.CurrentIndex >= _processviewModel.DisplayedDataList.Count)
            {
                return;
            }
            _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].BackgroundColor = Brushes.LightGreen;
            _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].Status = "Sent";

            _simPageUserControl2.Dispatcher.Invoke(() => // Reset the ItemsSource property of the ListView and update it with the displayed data list
            {
                _simPageUserControl2.listViewData.ItemsSource = null;
                _simPageUserControl2.listViewData.ItemsSource = _processviewModel.DisplayedDataList;
            });
            var webRequestHelper = new WebRequestHelper();
            await webRequestHelper.SendToComms(_processviewModel.CurrentIndex, _processviewModel.DisplayedDataList, _comboboxviewModel);
        }
    }
}