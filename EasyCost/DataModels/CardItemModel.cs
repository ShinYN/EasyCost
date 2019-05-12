using System.ComponentModel;

namespace EasyCost.DataModels
{
    public class CardItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaiseProperty(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; RaiseProperty(nameof(Selected)); }
        }

        public int Index { get; set; }

        public string CardName { get; set; }

        public string CardType { get; set; }

        public string Company { get; set; }

        public string CardNumber { get; set; }

        public string Description { get; set; }
    }
}
