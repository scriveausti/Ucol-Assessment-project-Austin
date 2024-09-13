using Microsoft.Win32;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace Assessment_project___Austin___23370104
{
    public partial class MainWindow : Window
    {
        public string[] search_resualts = { };

        public Dictionary<string,List<Dictionary<string, dynamic>>> loaded_lists = new Dictionary<string, List<Dictionary<string, dynamic>>>();

        //used in switching between pages 
        /// <summary>
        /// Updates the appearance and visibility of menu items and pages based on the selected menu option. 
        /// Changes the background and foreground colors of the menu items and toggles the visibility of the corresponding pages.
        /// </summary>
        /// <param name="selected">The menu item selected by the user. Valid values are "sales_page", "customer_page", and "checkout_page".</param>
        void Select_menu(string selected)
        {

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



        //Initialize Component
        public MainWindow()
        {
            InitializeComponent();

        }

        //Swich between pages
        private void menu_sales_page_Click(object sender, RoutedEventArgs e)
        {
            Select_menu("sales_page");
        }
        private void menu_customer_page_Click(object sender, RoutedEventArgs e)
        {
            Select_menu("customer_page");
        }
        private void menu_checkout_page_Click(object sender, RoutedEventArgs e)
        {
            Select_menu("checkout_page");
        }


        // updates the "wait_list" list box 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list_name"></param>
        public void update_wait_list(string list_name)
        {
            wait_list.Items.Clear();
            foreach(Dictionary<string,dynamic> product in loaded_lists[list_name])
            {
                wait_list.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: " + product["price"] + Environment.NewLine +  "Quantity: " + product["quantity"]);
            }
        }



        public void update_sales_list_selecter() 
        {
            sales_list_select.Items.Clear();
            foreach(string key in loaded_lists.Keys)
            {
                sales_list_select.Items.Add(key);
               
            }
        }

        //
        private void sales_list_select_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sales_list_select.SelectedItem != null)
            {
                update_wait_list(sales_list_select.SelectedItem.ToString());
            }
            else
            {
                wait_list.Items.Clear();
            }
        }


        //part A
        //part 1
        private void add_wait_list_Click(object sender, RoutedEventArgs e)
        {
            // stores all info as strings and then adds them to the waiting list and then clears the field 

            bool right_data_types = true;
            bool id_alread_used = false;


            List<Dictionary<string, dynamic>> product_list = new List<Dictionary<string, dynamic>>();
            Dictionary<string, dynamic> new_product = new Dictionary<string, dynamic>();

            string selcted_item;


            if (!(loaded_lists.ContainsKey("new_products")))
            {
                loaded_lists.Add("new_products", product_list);
            }
            if (sales_list_select.SelectedItem != null)
            {
                selcted_item =sales_list_select.SelectedItem.ToString();
            }
            else
            {
                sales_list_select.SelectedItem = "new_products";
                Trace.WriteLine(sales_list_select.SelectedItem);
                selcted_item = "new_products";
            }

            product_list = loaded_lists[selcted_item];


            foreach(Dictionary<string, dynamic> product in product_list)
            {
                if (product["id"] == input_product_id.Text)
                {
                    MessageBox.Show("Error, Product ID already in list", "Error", MessageBoxButton.OK);
                    right_data_types = false;
                    id_alread_used = true;
                }
            }
            if (!id_alread_used)
                new_product.Add("id", input_product_id.Text);



            new_product.Add("name", input_product_name.Text);

            try { new_product.Add("price", Convert.ToDouble(input_product_price.Text)); }
            catch (Exception)
            {
                MessageBox.Show("Error, please enter a Double (decimal) in the price box", "Error", MessageBoxButton.OK);
                right_data_types = false;
            }

            try { new_product.Add("quantity", Convert.ToInt32(input_product_quantity.Text)); }
            catch(Exception)
            {
                MessageBox.Show("Error, please enter a Interger (Whole number) in the quantity box", "Error", MessageBoxButton.OK);
                right_data_types = false;
            }

            if (right_data_types)
            {

                product_list.Add(new_product);
                loaded_lists[selcted_item] = product_list;
                update_wait_list(selcted_item);
                update_sales_list_selecter();
                sales_list_select.SelectedItem = selcted_item;

                input_product_id.Text = "";
                input_product_name.Text = "";
                input_product_price.Text = "";
                input_product_quantity.Text = "";
            }
        }

        //part 2 
        private void clear_list_Click(object sender, RoutedEventArgs e)
        //checks if the user has selected a item
        //if they haven't it will clear the whole list
        //other wise it'll just clear the item 
        {
            if (sales_list_select.SelectedItem != null)
            {
                if (wait_list.SelectedItem == null)
                {
                    loaded_lists[sales_list_select.SelectedItem.ToString()].Clear();
                    update_wait_list(sales_list_select.SelectedItem.ToString());
                }
                else
                {
                    
                    foreach (string item in wait_list.SelectedItems) 
                    {
                        Trace.WriteLine(item);
                        Trace.WriteLine(wait_list.Items.IndexOf(item));
                        loaded_lists[sales_list_select.SelectedItem.ToString()].RemoveAt(wait_list.Items.IndexOf(item));
                    }

                }
                update_wait_list(sales_list_select.SelectedItem.ToString());
            }
        }

        //part 3
        private void save_file_Click(object sender, RoutedEventArgs e)
        {
            if (sales_list_select.SelectedItem != null)
            {
                // saves all items in the products box to a file called "stored_list.txt"
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "txt files (*.txt)|*.txt";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string save_string = "";
                    int iteration_product = 0;
                    foreach (Dictionary<string, dynamic> product in loaded_lists[sales_list_select.SelectedItem.ToString()])
                    {
                        int iteration_attributes = 0;
                        foreach (string key in product.Keys)
                        {
                            if (iteration_attributes < 3)
                            {
                                save_string = save_string + product[key] + ",";
                            }
                            else
                            {
                                save_string = save_string + product[key];
                            }
                            iteration_attributes ++;
                        }
                        
                        if (iteration_product < (loaded_lists[sales_list_select.SelectedItem.ToString()].Count -1 ))
                            save_string = save_string + Environment.NewLine;
                        iteration_product++;

                    }

                    File.WriteAllText(saveFileDialog.FileName, save_string);
                }
            }
        }

        //part 4
        public void load_file_Click(object sender, RoutedEventArgs e)
        {
            string[] load_list = { };
            string[] load_product_array = { };


            bool file_error = false;

            List<Dictionary<string, dynamic>> formated_list = new List<Dictionary<string, dynamic>>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true) { 

                load_list = File.ReadAllText(openFileDialog.FileName).Split(Environment.NewLine);

                for (int i = 0; i < load_list.Length; i++)
                {
                    if (file_error == false)
                    {
                        load_product_array = load_list[i].Split(",");
                        if (load_product_array.Length == 4)
                        {

                            bool right_data_types = true;
                            bool id_alread_used = false;
                            Dictionary<string, dynamic> loading_product = new Dictionary<string, dynamic>();


                            foreach (Dictionary<string, dynamic> product in formated_list)
                            {
                                if (product["id"] == load_product_array[0])
                                {
                                    MessageBox.Show("Error, Product ID already in list", "Error", MessageBoxButton.OK);
                                    right_data_types = false;
                                    id_alread_used = true;
                                }
                            }
                            if (!id_alread_used)
                                loading_product.Add("id", load_product_array[0]);




                            loading_product.Add("name", load_product_array[1]);

                            try { loading_product.Add("price", Convert.ToDouble(load_product_array[2])); }
                            catch (Exception)
                            {
                                MessageBox.Show("Error,  data from file for a price is not the right data type", "Error", MessageBoxButton.OK);
                                right_data_types = false;
                            }

                            try { loading_product.Add("quantity", Convert.ToInt32(load_product_array[3])); }
                            catch (Exception)
                            {
                                MessageBox.Show("Error, data from file for a quantity is not the right data type", "Error", MessageBoxButton.OK);
                                right_data_types = false;
                            }

                            if (right_data_types)
                            {
                                formated_list.Add(loading_product);

                            }
                            else
                            {
                                file_error = true;
                            }

                        }
                        else
                        {
                            file_error = true;
                        }
                    }
                    
                }
                if (!file_error)
                {
                    string list_name = openFileDialog.FileName;

                    if (loaded_lists.ContainsKey(list_name))
                    {
                        if (MessageBox.Show("File selected alread loaded. Do you want to override it?", "Override", MessageBoxButton.YesNo) == MessageBoxResult.Yes) ;
                        {
                            loaded_lists.Remove(list_name);
                            loaded_lists.Add(list_name, formated_list);
                            sales_list_select.SelectedItem = list_name;
                            update_wait_list(list_name);
                            update_sales_list_selecter();
                        }
                    }
                    else
                    {
                        loaded_lists.Add(list_name, formated_list);
                        sales_list_select.SelectedItem = list_name;
                        update_wait_list(list_name);
                        update_sales_list_selecter();
                    }
                }
                else
                {
                    MessageBox.Show("Error, bad file, please retry with a difrent file", "Error", MessageBoxButton.OK);
                }
            }
        }

        
        //part B
        //part 1
        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            /*
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
                MessageBox.Show("Unable to find item", "Not Found", MessageBoxButton.OK);
            }
            */
        }

        private void place_order_Click(object sender, RoutedEventArgs e)
        {

        }

        private void view_all_products_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
            if (wait_list.SelectedItem != null)
            {

            }
        }


    }
}