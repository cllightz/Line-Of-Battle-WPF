using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineOfBattle.ViewModels
{
    public class UIViewModel
    {
        public string Test { get; set; } = "バインディング";
        public object SceneViewModel;

        public UIViewModel()
        {
            // _model = Model.Singleton;

            // _model = Global.MainLogic;
        }
    }
}
