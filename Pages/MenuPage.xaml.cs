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

namespace ViPKS_PR10_FirstTry.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void btnGoToEmployeesPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EmplyeesPage());
        }

        private void btnGoToEquipmentsPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EquipmentsPage());
        }

        private void btnGoToMaterialsPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MaterialsPage());
        }

        private void btnGoToOrderAssignmentsPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrderAssignmentsPage());
        }

        private void btnGoToOrdersPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new OrdersPage());
        }

        private void btnGoToProductsPage_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ProductsPage());
        }
    }
}
