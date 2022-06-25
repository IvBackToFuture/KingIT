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
    /// Логика взаимодействия для StatOfShopCenterPage.xaml
    /// </summary>
    public partial class StatOfShopCenterPage : Page
    {
        public StatOfShopCenterPage()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return "Интерфейс Статистика по торговому центру";
        }
    }
}
