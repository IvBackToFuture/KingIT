using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KingIT.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для ArendatorsCollectionPage.xaml
    /// </summary>
    public partial class ArendatorsCollectionPage : Page
    {
        public ArendatorsCollectionPage()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "Интерфейс Cписок арендаторов";
        }
    }
}
