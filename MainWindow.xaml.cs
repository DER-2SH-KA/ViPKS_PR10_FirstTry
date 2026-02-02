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
using ViPKS_PR10_FirstTry.Database;

namespace ViPKS_PR10_FirstTry
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Material _currentMaterial;
        private Product _currentProduct;
        private ProductionOrder _currentProductionOrder;

        public MainWindow()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            using (var db = new DatabaseEntities())
            {
                MaterialsGrid.ItemsSource = db.Material.ToList();
                ProductsGrid.ItemsSource = db.Product.ToList();
                OrdersGrid.ItemsSource = db.ProductionOrder
                    .Include("Order")
                    .Include("Order.OrderStatus1")
                    .Include("Order.Client")
                    .Include("Product")
                    .ToList();
                EmployeesGrid.ItemsSource = db.Employee.Include("Department").Include("EmployeePosition").ToList();
                EquipmentGrid.ItemsSource = db.Equipment.Include("EquipmentStatus").ToList();
                PurchasesGrid.ItemsSource = db.SupplierPurchase
                    .Include("Material")
                    .Include("Purchase")
                    .ToList();
                RemainsGrid.ItemsSource = db.MaterialRemains.Include("Material").ToList();

                ComboBoxMaterialUnits.ItemsSource = db.UnitOfMeasurement.ToList();
                ComboBoxSpecification.ItemsSource = db.Specification.ToList();
                ComboBoxProductionOrderProduct.ItemsSource = db.Product.ToList();
                ComboBoxProductionOrderOrder.ItemsSource = db.Order.ToList();
            }
        }

        // CRUD МАТЕРИАЛОВ.

        private void AddMaterial_Click(object sender, RoutedEventArgs e)
        {
            _currentMaterial = new Material();
            ShowMaterialForm();
        }

        private void MaterialsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MaterialsGrid.SelectedItem is Material selected)
            {
                _currentMaterial = selected;
                TxtMaterialTitle.Text = _currentMaterial.Title;
                TxtMatMinRemains.Text = _currentMaterial.MinRemains.ToString();
                ComboBoxMaterialUnits.SelectedItem = _currentMaterial.UnitOfMeasurement;
                ShowMaterialForm();
            }
        }

        private void btnSaveMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    _currentMaterial.Title = TxtMaterialTitle.Text;
                    _currentMaterial.MinRemains = float.Parse(TxtMatMinRemains.Text);
                    _currentMaterial.UnitOfMeasurementId = (ComboBoxMaterialUnits.SelectedItem as UnitOfMeasurement).Id;

                    if (_currentMaterial.Id == 0) db.Material.Add(_currentMaterial);
                    else db.Entry(_currentMaterial).State = System.Data.EntityState.Modified;

                    db.SaveChanges();
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void btnDeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    if (_currentMaterial != null)
                    {
                        Material entityToDelete = db.Material.Where(entity => _currentMaterial.Id == entity.Id).First();
                        db.Material.Remove(entityToDelete);
                        db.SaveChanges();

                        _currentMaterial = null;
                    }
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        // CRUD ПРОДУКТОВ.

        private void ProductsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsGrid.SelectedItem is Product selected)
            {
                _currentProduct = selected;
                TxtProdTitle.Text = _currentProduct.Title;
                TxtProdDescription.Text = _currentProduct.Description;
                ComboBoxSpecification.SelectedItem = _currentProduct.Specification;
                ShowProductForm();
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            _currentProduct = new Product();
            _currentProduct.Articul = 0L;
            ShowProductForm();
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    _currentProduct.Title = TxtProdTitle.Text.Trim();
                    _currentProduct.Description = TxtProdDescription.Text.Trim();
                    _currentProduct.SpecificationId = (ComboBoxSpecification.SelectedItem as Specification).Id;

                    if (_currentProduct.Articul == 0) db.Product.Add(_currentProduct);
                    else db.Entry(_currentProduct).State = System.Data.EntityState.Modified;

                    db.SaveChanges();
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    if (_currentProduct != null)
                    {
                        Product entityToDelete = db.Product.Where(entity => _currentProduct.Articul == entity.Articul).First();
                        db.Product.Remove(entityToDelete);
                        db.SaveChanges();

                        _currentProduct = null;
                    }
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        // CRUD ЗАКАЗОВ.

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is ProductionOrder selected)
            {
                _currentProductionOrder = selected;
                TxtProductionOrderQuantity.Text = "1";
                ComboBoxProductionOrderOrder.SelectedItem = selected.Order;
                ComboBoxProductionOrderProduct.SelectedItem = selected.Product;

                ShowOrderForm();
            }
        }

        private void AddProductionOrder_Click(object sender, RoutedEventArgs e)
        {
            _currentProductionOrder = new ProductionOrder();
            ShowOrderForm();
        }

        private void btnSaveProductionOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    _currentProductionOrder.Quantity = float.Parse(TxtProductionOrderQuantity.Text);
                    _currentProductionOrder.OrderId = (ComboBoxProductionOrderOrder.SelectedItem as Order).Id;
                    _currentProductionOrder.ProductionArticul = (ComboBoxProductionOrderProduct.SelectedItem as Product).Articul;

                    if (_currentProductionOrder.Id == 0) db.ProductionOrder.Add(_currentProductionOrder);
                    else db.Entry(_currentProductionOrder).State = System.Data.EntityState.Modified;

                    db.SaveChanges();
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void btnDeleteProductionOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DatabaseEntities())
                {
                    if (_currentProduct != null)
                    {
                        Product entityToDelete = db.Product.Where(entity => _currentProduct.Articul == entity.Articul).First();
                        db.Product.Remove(entityToDelete);
                        db.SaveChanges();

                        _currentProduct = null;
                    }
                }
                LoadAllData();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        // ВСПОМОГАТЕЛЬНАЯ ЛОГИКА.

        private void ShowMaterialForm()
        {
            MaterialEditForm.Visibility = Visibility.Visible;
            ProductEditForm.Visibility = Visibility.Collapsed;
        }

        private void ShowProductForm()
        {
            ProductEditForm.Visibility = Visibility.Visible;
            MaterialEditForm.Visibility = Visibility.Collapsed;
        }

        private void ShowOrderForm()
        {
            ProductionOrderEditForm.Visibility = Visibility.Visible;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MaterialEditForm.Visibility = Visibility.Collapsed;
            ProductEditForm.Visibility = Visibility.Collapsed;
            ProductionOrderEditForm.Visibility = Visibility.Collapsed;
        }
    }
}
