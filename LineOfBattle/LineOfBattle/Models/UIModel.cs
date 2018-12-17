using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineOfBattle.Models
{
    internal class UIModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static UIModel _singleton;
        internal static UIModel Singleton => _singleton ?? new UIModel();

        internal object SceneModelInstance;

        private UIModel() { }
    }
}
