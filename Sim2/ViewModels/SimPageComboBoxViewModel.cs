using System.Collections.Generic;
using System.ComponentModel;

namespace Sim2.ViewModels
{
    public class SimPageComboBoxViewModel : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}