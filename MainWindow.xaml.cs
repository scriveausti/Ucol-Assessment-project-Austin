using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Media;


namespace Assessment_project___Austin___23370104
{

    public partial class MainWindow : Window
    {

        public string[] search_resualts = { };

        public Dictionary<string, List<Dictionary<string, dynamic>>> LoadedLists = new Dictionary<string, List<Dictionary<string, dynamic>>>();

        public List<Dictionary<string, dynamic>> search_resualts_list = new List<Dictionary<string, dynamic>>();

        public List<Dictionary<string, dynamic>> cart = new List<Dictionary<string, dynamic>>();

        public double total_cart_cost = new int();

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
            Trace.WriteLine("test");
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
            foreach (Dictionary<string, dynamic> product in LoadedLists[list_name])
            {
                wait_list.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
            }
        }

        public void update_product_veiw(List<Dictionary<string, dynamic>> list)
        {
            product_view.Items.Clear();
            foreach (Dictionary<string, dynamic> product in list)
            {
                product_view.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
            }
        }

        public void update_cart()
        {
            cart_list_box.Items.Clear();
            checkout_cart.Items.Clear();
            total_cart_cost = 0;
            foreach (Dictionary<string, dynamic> product in cart)
            {
                cart_list_box.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
                checkout_cart.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
                total_cart_cost = total_cart_cost + (product["quantity"] * product["price"]);
            }
            Total_price.Text = "$" + total_cart_cost.ToString("N2");
        }

        public void update_list_selecter()
        {
            customer_list_select.Items.Clear();
            sales_list_select.Items.Clear();
            foreach (string key in LoadedLists.Keys)
            {
                customer_list_select.Items.Add(key);
                sales_list_select.Items.Add(key);

            }

        }

        public void update_all()
        {
            update_wait_list("product_list");
            update_cart();
            update_list_selecter();
        }

        public void load_file(string fileName)
        {
            string[] load_list = { };
            string[] load_product_array = { };

            List<Dictionary<string, dynamic>> formated_list = new List<Dictionary<string, dynamic>>();

            StreamReader loadedFile = new StreamReader(fileName);
            load_list = loadedFile.ReadToEnd().Split(Environment.NewLine);
            if (load_list[0] =="")
            {
                MessageBox.Show("File is empty", "File Empty", MessageBoxButton.OK);
                return;
            }
            



            for (int i = 0; i < load_list.Length; i++)
            {
                load_product_array = load_list[i].Split(",");


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
            }
            
            Trace.Write(formated_list.ToString());
            LoadedLists["product_list"] = formated_list;
            clear_cart();
            update_all();
            sales_list_select.SelectedItem = "product_list";
            loadedFile.Close();
        }

        public void save_file(string fileName, string listName)
        {
            if (sales_list_select.SelectedItem == null)
            {
                return;
            }
            // saves all items in the products box to a file called "stored_list.txt"
            
            string save_string = "";
            int iteration_product = 0;
            foreach (Dictionary<string, dynamic> product in LoadedLists[listName])
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
                    iteration_attributes++;
                }

                if (iteration_product < (LoadedLists[ "product_list"].Count - 1))
                    save_string = save_string + Environment.NewLine;
                iteration_product++;

            }

            StreamWriter loadedFile = new StreamWriter(fileName);
            loadedFile.Write( save_string);
            loadedFile.Close();
        }

        public void clear_cart()
        {
            cart.Clear();
            update_cart();
        }



        private void sales_list_select_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sales_list_select.SelectedItem != null)
            {
                update_wait_list( "product_list");
                update_product_veiw(LoadedLists[ "product_list"]);
                customer_list_select.SelectedItem = sales_list_select.SelectedItem;
            }
            else
            {
                wait_list.Items.Clear();
            }
        }

        private void customer_list_select_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sales_list_select.SelectedItem != null)
            {
                update_wait_list( "product_list");
                update_product_veiw(LoadedLists[ "product_list"]);
                sales_list_select.SelectedItem = customer_list_select.SelectedItem;
            }
            else
            {
                wait_list.Items.Clear();
                product_view.Items.Clear();
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
            if ((input_product_id.Text == "") || (input_product_name.Text == "") || (input_product_price.Text == "") || (input_product_quantity.Text == ""))
            {
                MessageBox.Show("Error, Text box empty", "Error", MessageBoxButton.OK);
                return;
            }

            if (!(LoadedLists.ContainsKey("new_products")))
            {
                LoadedLists.Add("new_products", product_list);
            }
            if (sales_list_select.SelectedItem != null)
            {
                selcted_item =  "product_list";
            }
            else
            {
                sales_list_select.SelectedItem = "product_list";
                Trace.WriteLine(sales_list_select.SelectedItem);
                selcted_item = "product_list";
            }


            product_list = LoadedLists[selcted_item];


            foreach (Dictionary<string, dynamic> product in product_list)
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
            catch (Exception)
            {
                MessageBox.Show("Error, please enter a Interger (Whole number) in the quantity box", "Error", MessageBoxButton.OK);
                right_data_types = false;
            }

            if (right_data_types)
            {

                product_list.Add(new_product);
                LoadedLists[selcted_item] = product_list;
                update_wait_list(selcted_item);
                update_list_selecter();
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
            if (sales_list_select.SelectedItem == null)
            {
                return;
            }
            if (wait_list.SelectedItem == null)
            {
                LoadedLists[ "product_list"].Clear();
                update_wait_list( "product_list");
            }
            else
            {
                foreach (string item in wait_list.SelectedItems)
                {
                    Trace.WriteLine(item);
                    Trace.WriteLine(wait_list.Items.IndexOf(item));
                    LoadedLists[ "product_list"].RemoveAt(wait_list.Items.IndexOf(item));
                }
            }
            update_wait_list( "product_list");
        }

        //part 3
        private void save_file_Click(object sender, RoutedEventArgs e)
        {
            save_file("ProductFile.txt" , "product_list");
        }

        //part 4
        public void load_file_Click(object sender, RoutedEventArgs e)
        {
            load_file("ProductFile.txt");
        }


        //part B
        //part 1
        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.SelectedItem != null)
            {
                bool Item_found = false;
                product_view.Items.Clear();
                string search_item = input_search_product.Text;
                search_resualts_list.Clear();

                foreach (Dictionary<string, dynamic> product in LoadedLists[ "product_list"])

                    if (product["name"].Contains(search_item) || product["id"].Contains(search_item))
                    {
                        search_resualts_list.Add(product);
                        Item_found = true;
                    }

                if (!Item_found)
                {
                    MessageBox.Show("Unable to find item", "Not Found", MessageBoxButton.OK);
                }
                else
                {
                    update_product_veiw(search_resualts_list);
                }
            }
            else
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK);
            }

        }

        //part 4
        private void place_order_Click(object sender, RoutedEventArgs e)
        {
            Select_menu("checkout_page");
        }
        
        //part 2
        private void view_all_products_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.Items.Count == 0)
            {
                load_file("ProductFile.txt");
                customer_list_select.SelectedItem = "product_list";
            }
            if (customer_list_select.SelectedItem == null)
            {
                return;
            }
            update_product_veiw(LoadedLists[ "product_list"]);
        }

        //part 3
        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.SelectedItem == null)
            {
                return;
            }

            if (product_view.SelectedItem == null)
            {
                return;
            }

            int add_ammount = 0;

            if (cart_add_amount.Text == "")
            {
                cart_add_amount.Text = "1";
            }

            try { add_ammount = Convert.ToInt32(cart_add_amount.Text); }
            catch (Exception)
            {
                MessageBox.Show("Error, please enter a Interger (Whole number) in the box", "Error", MessageBoxButton.OK);
                return;
            }

            List<Dictionary<string, dynamic>> target_list = new List<Dictionary<string, dynamic>>();

            if (search_resualts_list.Count() != 0) // if an item has been searched for
            {
                target_list = search_resualts_list; //pull from search resualts 
            }
            else
            {
                target_list = LoadedLists[ "product_list"]; //pull from the selected product list
            }

            Dictionary<string, dynamic> product = target_list[product_view.SelectedIndex];

            if (add_ammount > product["quantity"])
            {
                MessageBox.Show("The amount you want to add is greater then the avalible ammount", "Limit reached", MessageBoxButton.OK);
                return;
            }

            bool item_not_found = true;
            foreach (Dictionary<string, dynamic> cart_item in cart)
            {
                if (cart_item["id"] != product["id"]) //finds item in cart
                {
                    continue;
                }

                item_not_found = false;

                if ((add_ammount + cart_item["quantity"]) > product["quantity"])
                {
                    MessageBox.Show("The amount you want to add is greater then the avalible ammount", "Limit reached", MessageBoxButton.OK);
                    return;
                }

                if (cart_item["quantity"] != product["quantity"]) // checks if the 
                {
                    cart_item["quantity"] += add_ammount;
                    break;
                }

                else
                {
                    MessageBox.Show("Limit reached", "Limit reached", MessageBoxButton.OK);
                    break;
                }
            }

            if (item_not_found)
            {
                Dictionary<string, dynamic> product_info = new Dictionary<string, dynamic>(product);
                product_info["quantity"] = add_ammount;
                cart.Add(product_info);
            }
            update_cart();

        }

        private void remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if (cart_list_box.SelectedItem == null)
            {
                return;
            }
            cart.RemoveAt(cart_list_box.SelectedIndex);
            update_cart();
        }

        private void clear_cart_Click(object sender, RoutedEventArgs e)
        {
            clear_cart();
        }


        //part C
        //part 1
        private void btn_checkout_Click(object sender, RoutedEventArgs e)
        {
            bool empty_box = false;
            if (checkout_name_box.Text == "") empty_box = true;
            if (checkout_contact_number_box.Text == "") empty_box = true;
            if (checkout_email_box.Text == "") empty_box = true;
            if (checkout_address_box.Text == "") empty_box = true;
            if (empty_box)
            {
                MessageBox.Show("Error, Text box empty", "Error", MessageBoxButton.OK);
                return;
            }
            MessageBox.Show("Thank You", "Thank You", MessageBoxButton.OK);



            List<Dictionary<string, dynamic>> updatedList = LoadedLists["product_list"];
            foreach ( Dictionary<string,dynamic> orderedProduct in cart)
            {
                string idToFind = orderedProduct["id"];
                foreach (Dictionary<string, dynamic> savedProduct in LoadedLists["product_list"])
                {
                    if (idToFind != savedProduct["id"])
                    {
                        continue;
                    }

                    savedProduct["quantity"] = savedProduct["quantity"] - orderedProduct["quantity"];
                    break;

                }

            }

            LoadedLists["product_list"] = updatedList;
            save_file("productFile.txt", "product_list");

        }

        private void btn_edit_cart_Click(object sender, RoutedEventArgs e)
        {
            Select_menu("customer_page");
        }


    }
}