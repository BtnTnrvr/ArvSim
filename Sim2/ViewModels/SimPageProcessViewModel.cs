using Sim2.Helper;
using Sim2.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Sim2.ViewModels
{
    public class SimPageProcessViewModel : INotifyPropertyChanged
    {
        public SimPageProcessViewModel() { }
        public SimPageProcessViewModel(List<PacketModel> items)
        {
            _displayedDataList = items.ToList();
        }
        public List<string> RegionList { get; set; }
        public List<string> Region1List { get; set; }
        public List<string> Region2List { get; set; }
        public List<string> Region3List { get; set; }
        public List<string> Region4List { get; set; }
        public List<string> Region5List { get; set; }
        public MessageBoxResult result { get; set; }

        public void PopulateRegionLists(List<PacketModel> items)
        {
            RegionList = RegionHelper.GetDistinctRegions(items); // Getting the how many different region for each region and collecting them in to one list
            Region1List = RegionHelper.GetREGION(items);
            Region2List = RegionHelper.GetREGION2(items);
            Region3List = RegionHelper.GetREGION3(items);
            Region4List = RegionHelper.GetREGION4(items);
            Region5List = RegionHelper.GetREGION5(items);

            items = RegionHelper.ConvertRegionNames(items, RegionList); // Getting ConvertRegion method from the RegionHelper
        }
        private int _currentIndex;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }
        private List<PacketModel> _displayedDataList;
        public List<PacketModel> DisplayedDataList
        {
            get { return _displayedDataList; }
            set { _displayedDataList = value; }
        }
        private bool _hasShownMessageBox = false;
        public bool HasShownMessageBox
        {
            get { return _hasShownMessageBox; }
            set
            {
                _hasShownMessageBox = value;
                OnPropertyChanged(nameof(HasShownMessageBox));
            }
        }
        private bool _reverseloopEnabled;
        public bool ReverseloopEnabled
        {
            get { return _reverseloopEnabled; }
            set
            {
                _reverseloopEnabled = value;
                OnPropertyChanged(nameof(ReverseloopEnabled));
            }
        }
        private bool _forwardloopEnabled;
        public bool ForwardloopEnabled
        {
            get { return _forwardloopEnabled; }
            set
            {
                _forwardloopEnabled = value;
                OnPropertyChanged(nameof(ForwardloopEnabled));
            }
        }
        private bool _stopAfterIteration = false;
        public bool StopAfterIteration
        {
            get { return _stopAfterIteration; }
            set
            {
                _stopAfterIteration = value;
                OnPropertyChanged(nameof(StopAfterIteration));
            }
        }
        private bool _isIterationContinue = false;
        public bool IsIterationContinue
        {
            get { return _isIterationContinue; }
            set
            {
                _isIterationContinue = value;
                OnPropertyChanged(nameof(IsIterationContinue));
            }
        }
        private bool _isPaused = false;
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }
        private bool _isActive = true;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
        private List<string> _chosenTestexas = new List<string>();
        public List<string> ChosenTestexas
        {
            get { return _chosenTestexas; }
            set
            {
                _chosenTestexas = value;
                OnPropertyChanged(nameof(ChosenTestexas));
            }
        }

        private string _packageUrl;
        public string PackageUrl
        {
            get { return _packageUrl; }
            set
            {
                _packageUrl = value;
                OnPropertyChanged(nameof(PackageUrl));
            }
        }
        private static Dictionary<int, List<string>> _selectedComboItemsPerTab = new Dictionary<int, List<string>>();
        public Dictionary<int, List<string>> SelectedComboItemsPerTab
        {
            get { return _selectedComboItemsPerTab; }
            set 
            { 
                _selectedComboItemsPerTab = value;
                OnPropertyChanged(nameof(SelectedComboItemsPerTab));
            }
        }        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}