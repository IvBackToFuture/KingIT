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
    /// Логика взаимодействия для PavilionsCollectionPage.xaml
    /// </summary>
    public partial class PavilionsCollectionPage : Page
    {
        public PavilionsCollectionPage()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "Список павильонов";
        }
    }
}
