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
        private SimPageUserControl2 _simPageUserControl2;
        public ProcessHelper(List<PacketModel> items, SimPageUserControl2 simPageUserControl2, SimPageProcessViewModel processViewModel)
        {
            _simPageUserControl2 = simPageUserControl2;
            _processviewModel = new SimPageProcessViewModel(items);
            _simPageUserControl2.listViewData.ItemsSource = _processviewModel.DisplayedDataList;
            _processviewModel = processViewModel;
        }
        public void ReverseLoop()
        {
            Looper();
            ResetBackgroundListView();

            for (int i = _processviewModel.DisplayedDataList.Count - 1; i >= -1; i--)
            {
                UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                if (_processviewModel.CurrentIndex >= 0 && _processviewModel.CurrentIndex < _processviewModel.DisplayedDataList.Count)
                {
                    Thread.Sleep(_processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].TimeDifferenceAsTimeSpan);
                }
            }
            ResetBackgroundListView();

            if (_processviewModel.StopAfterIteration == true || _processviewModel.DisplayedDataList.Count == 1) // Checking for the iteration comes to the end in reverse loop if its, brushing it green so iteration could stop in a order
            {
                _processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].BackgroundColor = Brushes.LightGreen;
            }
            UpdateBackgroundListViewAsync();
            ResetBackgroundListView();
        }
        public void ForwardLoop()
        {
            Looper();
            ResetBackgroundListView();

            if (_processviewModel.CurrentIndex == _processviewModel.DisplayedDataList.Count) // Check if the end of the list has been reached and reset the index
            {
                _processviewModel.CurrentIndex = 0;
            }
        }
        public void NormalProgressAsync()
        {
            Looper();
            UpdateBackgroundListViewAsync();
        }
        private void ResetBackgroundListView()
        {
            foreach (var data in _processviewModel.DisplayedDataList) // Reset the background color and status text of all the rows in the displayed data list
            {
                data.BackgroundColor = Brushes.White;
                data.Status = string.Empty;
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
        private void UpdateBackgroundListViewAsync()
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
            webRequestHelper.SendToComms(_processviewModel.CurrentIndex, _processviewModel.DisplayedDataList, _processviewModel);
        }
        private void Looper()
        {
            for (int i = 0; i < _processviewModel.DisplayedDataList.Count; i++)
            {
                UpdateBackgroundListViewAsync();
                _processviewModel.CurrentIndex = i;
                MoveToTheNextLine();
                if (_processviewModel.CurrentIndex >= 0 && _processviewModel.CurrentIndex < _processviewModel.DisplayedDataList.Count)
                {
                    Thread.Sleep(_processviewModel.DisplayedDataList[_processviewModel.CurrentIndex].TimeDifferenceAsTimeSpan);
                }
            }
        }
    }
}