using Microsoft.Win32;
using System.IO;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assessment_project___Austin___23370104
{
    public partial class MainWindow : Window
    {
        public string[] load_list = { };
        public string[] search_resualts = { };


        //used in switching between pages 
        void select_menu(string selected)
        {
            /// <summary>
            /// highlights the selected menu button and make the page visable 
            /// inputs "sales_page", "customer_page", "checkout_page"
            /// </summary>
            if (selected == "sales_page")
            {
                menu_sales_page.Background = new SolidColorBrush(Colors.Black);
                menu_sales_page.Foreground = new SolidColorBrush(Colors.White);
                sales_page.Visibility = Visibility.Visible;
            }
            else
            {
                menu_sales_page.Background = new SolidColorBrush(Colors.White);
                menu_sales_page.Foreground = new SolidColorBrush(Colors.Black);
                sales_page.Visibility = Visibility.Hidden;
            }
            if (selected == "customer_page")
            {
                menu_customer_page.Background = new SolidColorBrush(Colors.Black);
                menu_customer_page.Foreground = new SolidColorBrush(Colors.White);
                customer_page.Visibility = Visibility.Visible;
            }
            else
            {
                menu_customer_page.Background = new SolidColorBrush(Colors.White);
                menu_customer_page.Foreground = new SolidColorBrush(Colors.Black);
                customer_page.Visibility = Visibility.Hidden;
            }
            if (selected == "checkout_page")
            {
                menu_checkout_page.Background = new SolidColorBrush(Colors.Black);
                menu_checkout_page.Foreground = new SolidColorBrush(Colors.White);
                checkout_page.Visibility = Visibility.Visible;
            }
            else
            {
                menu_checkout_page.Background = new SolidColorBrush(Colors.White);
                menu_checkout_page.Foreground = new SolidColorBrush(Colors.Black);
                checkout_page.Visibility = Visibility.Hidden;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

        }

        
            //Swich between pages
        private void menu_sales_page_Click(object sender, RoutedEventArgs e)
        {
            select_menu("sales_page");
        }
        private void menu_customer_page_Click(object sender, RoutedEventArgs e)
        {
            select_menu("customer_page");
        }
        private void menu_checkout_page_Click(object sender, RoutedEventArgs e)
        {
            select_menu("checkout_page");
        }

        //part A
            //part 1
        private void add_wait_list_Click(object sender, RoutedEventArgs e)
        {
            // stores all info as strings and then adds them to the waiting list and then clears the field 
            string id_number = input_product_id.Text;
            string name = input_product_name.Text;
            string price = input_product_price.Text;
            string quantity = input_product_quantity.Text;
            wait_list.Items.Add("Id Number: " + id_number + " | Product Name: " + name + " | Price: " + price + " | Quantity: " + quantity +",");

            input_product_id.Text = "";
            input_product_name.Text = "";
            input_product_price.Text = "";
            input_product_quantity.Text = "";
        }

            //part 2
        private void clear_list_Click(object sender, RoutedEventArgs e)
        //checks if the user has selected a item
        //if they haven't it will clear the whole list
        //other wise it'll just clear the item 
        {
            if (wait_list.SelectedItem == null)
            {
                wait_list.Items.Clear();
            }
            else
            {
                wait_list.Items.Remove(wait_list.SelectedItem);
            }
        }

        //part 3
        private void save_file_Click(object sender, RoutedEventArgs e)
        {
            // saves all items in the products box to a file called "stored_list.txt"
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string save_string = "";
                for (int i = 0; i < wait_list.Items.Count; i++)
                {
                    save_string = save_string + wait_list.Items.GetItemAt(i).ToString();
                }
                File.WriteAllText(saveFileDialog.FileName, save_string);
            }
        }

            //part 4
        public void load_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                load_list = File.ReadAllText(openFileDialog.FileName).Split(',');

            for (int i = 0; i < load_list.Length; i++)
            {
                if (load_list[i] != "")
                {
                    load_list[i] = load_list[i] + ",";
                    if (load_list[i] != ",")
                    {
                        wait_list.Items.Add(load_list[i]);
                    }
                }
            }
        }

        //part B
            //part 1
        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            bool Item_found = false;
            product_view.Items.Clear();
            string search_item = input_search_product.Text;
            using (StreamReader readtext = new StreamReader("stored_list.txt"))
            {
                load_list = readtext.ReadToEnd().Split(',');
                for (int i = 0; i < load_list.Length; i++) load_list[i] = load_list[i] + ",";
            }
            for (int i = 0; i < load_list.Length; i++)
            {
                if (load_list[i].Contains(search_item))
                {
                    product_view.Items.Add(load_list[i]);
                    Item_found = true;
                }
            }
            if (!Item_found)
            {
                MessageBox.Show("Unable to find item", "Not Found");
            }

        }

        private void place_order_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void view_all_products_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}