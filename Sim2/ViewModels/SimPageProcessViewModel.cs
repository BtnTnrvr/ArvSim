using Sim2.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sim2.ViewModels
{
    public class SimPageProcessViewModel : INotifyPropertyChanged
    {
        public SimPageProcessViewModel(List<PacketModel> items)
        {
            _displayedDataList = items.ToList();
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
        public bool _isPaused = false;
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                OnPropertyChanged(nameof(IsPaused));
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}