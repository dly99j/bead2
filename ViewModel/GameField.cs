using System;

namespace bead2.ViewModel
{
    public class GameField : ViewModelBase
    {
        private String _text;

        private Boolean _player;
        private Boolean _guard;
        private Boolean _tree;
        private Boolean _food;
        private Boolean _empty;

        /// <summary>
        /// Felirat lekérdezése, vagy beállítása.
        /// </summary>
        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                    setText();
                }
            }
        }

        /// <summary>
        /// Vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 X { get; set; }

        /// <summary>
        /// Függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 Y { get; set; }

        /// <summary>
        /// Sorszám lekérdezése.
        /// </summary>
        public Int32 Number { get; set; }

        public Boolean IsEmpty
        { 
            get { return _empty; } 
            set
            {
                if (_empty != value)
                {
                    _empty = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsGuard
        { 
            get { return _guard; }
            set
            {
                if (_guard != value)
                {
                    _guard = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsFood
        { 
            get { return _food; }
            set
            {
                if (_food != value)
                {
                    _food = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsPlayer
        {
            get { return _player; }
            set
            {
                if (_player != value)
                {
                    _player = value;
                    OnPropertyChanged();
                }
            }
        }
        public Boolean IsTree
        { 
            get { return _tree; }
            set
            {
                if (_tree != value)
                {
                    _tree = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Lépés parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand StepCommand { get; set; }

        public void setFalse()
        {
            IsEmpty = false;
            IsPlayer = false;
            IsGuard = false;
            IsTree = false;
            IsFood = false;
        }

        public void setText()
        {
            setFalse();

            switch (_text)
            {
                case "E":
                    IsEmpty = true;
                    break;
                case "P":
                    IsPlayer = true;
                    break;
                case "T":
                    IsTree = true;
                    break;
                case "G":
                    IsGuard = true;
                    break;
                case "F":
                    IsFood = true;
                    break;
            }
        }
    }
}
