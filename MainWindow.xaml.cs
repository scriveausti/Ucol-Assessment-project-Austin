using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;


namespace Assessment_project___Austin___23370104
{

    public partial class MainWindow : Window
    {

        public string[] search_resualts = { };

        public Dictionary<string, List<Dictionary<string, dynamic>>> LoadedLists = new Dictionary<string, List<Dictionary<string, dynamic>>>();

        public List<Dictionary<string, dynamic>> searchResualtsList = new List<Dictionary<string, dynamic>>();

        public List<Dictionary<string, dynamic>> cart = new List<Dictionary<string, dynamic>>();

        public double totalCartCost = new int();

        //used in switching between pages 
        /// <summary>
        /// Updates the appearance and visibility of menu items and pages based on the selected menu option. 
        /// <para> Changes the background and foreground colors of the menu items and toggles the visibility of the corresponding pages.</para>
        /// </summary>
        /// <param name="selected">The menu item selected by the user. Valid values are "sales_page", "customer_page", and "checkout_page".</param>
        void SelectMenu(string selected)
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
            SelectMenu("sales_page");
        }
        private void menu_customer_page_Click(object sender, RoutedEventArgs e)
        {
            SelectMenu("customer_page");
        }
        private void menu_checkout_page_Click(object sender, RoutedEventArgs e)
        {
            SelectMenu("checkout_page");
        }

        /// <summary>
        /// Updates the wait list display with product information from a specified list.
        /// Clears the existing items in the wait list, then iterates through each product
        /// in the selected list (specified by <paramref name="listName"/> in LoadedLists)
        /// and adds formatted details (ID, Name, Price, Quantity) for each product to the wait list.
        /// </summary>
        /// <param name="listName">The name of the list in LoadedLists to be displayed in the wait list.</param>
        public void UpdateWaitList(string listName)
        {
            wait_list.Items.Clear();
            foreach (Dictionary<string, dynamic> product in LoadedLists[listName])
            {
                wait_list.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
            }
        }

        /// <summary>
        /// Updates the product view display with a list of product information.
        /// Clears the existing items in the product view, then iterates through each product
        /// in the provided list and adds formatted details (ID, Name, Price, Quantity) 
        /// for each product to the view.
        /// </summary>
        /// <param name="list">A list of dictionaries, each containing details of a product.</param>
        public void UpdateProductView(List<Dictionary<string, dynamic>> list)
        {
            product_view.Items.Clear();
            foreach (Dictionary<string, dynamic> product in list)
            {
                product_view.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
            }
        }

        /// <summary>
        /// Updates the cart display by clearing existing items and populating it with
        /// current products in the cart. Adds each product's details (ID, Name, Price, Quantity)
        /// to both the `cart_list_box` and `checkout_cart` views. Also calculates the total
        /// cost of items in the cart and updates the `Total_price` display.
        /// </summary>
        public void UpdateCart()
        {
            cart_list_box.Items.Clear();
            checkout_cart.Items.Clear();
            totalCartCost = 0;
            foreach (Dictionary<string, dynamic> product in cart)
            {
                cart_list_box.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
                checkout_cart.Items.Add(" ID: " + product["id"] + Environment.NewLine + "Name: " + product["name"] + Environment.NewLine + "Price: $" + product["price"].ToString("N2") + Environment.NewLine + "Quantity: " + product["quantity"]);
                totalCartCost = totalCartCost + (product["quantity"] * product["price"]);
            }
            Total_price.Text = "$" + totalCartCost.ToString("N2");
        }

        /// <summary>
        /// Updates the list selectors (`customer_list_select` and `sales_list_select`) with the keys from `LoadedLists`.
        /// Clears any existing items in both selectors, then adds each key from `LoadedLists`
        /// to both `customer_list_select` and `sales_list_select`.
        /// </summary>
        public void UpdateListSelecter()
        {
            customer_list_select.Items.Clear();
            sales_list_select.Items.Clear();
            foreach (string key in LoadedLists.Keys)
            {
                customer_list_select.Items.Add(key);
                sales_list_select.Items.Add(key);
            }
        }

        /// <summary>
        /// Updates multiple views by calling individual update methods.
        /// Refreshes the wait list with products from "product_list",
        /// updates the cart display, and refreshes the list selectors.
        /// </summary>
        public void UpdateAll()
        {
            UpdateListSelecter();
            UpdateWaitList(sales_list_select.SelectedItem.ToString());
            UpdateCart();
        }

        /// <summary>
        /// Loads product data from a specified file and populates the product list with valid entries.
        /// Reads file contents line-by-line, validates data, and checks for duplicates before adding products
        /// to a formatted list (`formated_list`). If the file is empty or contains invalid data types, appropriate
        /// error messages are shown. Upon successful loading, the product list in `LoadedLists` is updated, 
        /// the cart is cleared, and all displays are refreshed. The "product_list" is also set as the selected item
        /// in `sales_list_select`.
        /// </summary>
        /// <param name="fileName">The path to the file to be loaded.</param>
        public void LoadFile(string fileName)
        {
            string[] load_list = { };
            string[] load_product_array = { };

            List<Dictionary<string, dynamic>> formated_list = new List<Dictionary<string, dynamic>>();

            try
            {
                StreamReader loadedFile = new StreamReader(fileName); //loads the file with the name fileName
                load_list = loadedFile.ReadToEnd().Split(Environment.NewLine); //reads all lines in the loadedFile and then splits every new line (every new line is a new product)
                loadedFile.Close(); // closes the file
            }
            catch (Exception) //runs if any errors occurre 
            {
                MessageBox.Show("file missing please save a file before loading again", "Error", MessageBoxButton.OK); //error message 
                return; //ends the function
            }

            if (load_list[0] == "") // checks if the file is empty
            {
                MessageBox.Show("File is empty", "File Empty", MessageBoxButton.OK);
                return; //ends the function
            }

            for (int i = 0; i < load_list.Length; i++) // loops for the amount of products that are being loaded
            {
                load_product_array = load_list[i].Split(","); // splits the products into there infomation


                Dictionary<string, dynamic> loading_product = new Dictionary<string, dynamic>();


                foreach (Dictionary<string, dynamic> product in formated_list) // loops though all the products that are in forpated_list
                {
                    if (product["id"] == load_product_array[0]) // checks if the product id that the user has enterd is already in the list 
                    {
                        MessageBox.Show("Error, Product ID already in list", "Error", MessageBoxButton.OK); // tells user that the id alread in the list
                        return;// ends the function

                    }
                }
                
                loading_product.Add("id", load_product_array[0]); //store product infoation in to the Dictionary with the key "id"

                loading_product.Add("name", load_product_array[1]); //store product infoation in to the Dictionary with the key "name"

                try { loading_product.Add("price", Convert.ToDouble(load_product_array[2])); } // trys to convert the product infoation in to a double and store it to the Dictionary with the key "price"
                catch (Exception) // if an error occurrs in the convertion 
                {
                    MessageBox.Show("Error,  data from file for a price is not the right data type", "Error", MessageBoxButton.OK); // tells the user and error has occurred 
                    return; // ends the function
                }

                try { loading_product.Add("quantity", Convert.ToInt32(load_product_array[3])); } // trys to convert the product infoation in to a int and store it to the Dictionary with the key "quantity"
                catch (Exception) // if an error occurrs in the convertion 
                {
                    MessageBox.Show("Error, data from file for a quantity is not the right data type", "Error", MessageBoxButton.OK); // tells the user and error has occurred 
                    return; // ends the function
                }

                formated_list.Add(loading_product); // adds the product to the formated_list
            }

            LoadedLists[fileName] = formated_list; // saves the list to the big list 
            ClearCart(); // clears the cart
            UpdateListSelecter(); // updates all the display boxes
            sales_list_select.SelectedItem = fileName; // selects the new list if there is one created 
            UpdateWaitList(sales_list_select.SelectedItem.ToString());
        }

        /// <summary>
        /// Saves the products from a specified list (`listName`) to a file.
        /// Constructs a formatted string for each product with values separated by commas.
        /// If an item is not selected in `sales_list_select`, the method exits without saving.
        /// Each product's attributes are added line-by-line to the save string, separated by commas.
        /// Finally, writes the entire save string to a file with the specified `fileName`.
        /// </summary>
        /// <param name="fileName">The name of the file to save the product data to.</param>
        /// <param name="listName">The name of the list in `LoadedLists` to be saved.</param>
        public void SaveFile(string fileName, string listName)
        {
            if (sales_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK); // tells the user to select a list
                return; //ends the function
            }

            string save_string = "";
            int iteration_product = 0; // set iteration_product to 0

            foreach (Dictionary<string, dynamic> product in LoadedLists[listName]) // loops though all products in the list that is name is passed though 
            {
                int iteration_attributes = 0; // reset iteration_attributes to 0

                foreach (string key in product.Keys) // loops though the keys of the all the products
                {
                    if (iteration_attributes < 3) // checks if iteration_attributes is less then 3
                    {
                        save_string = save_string + product[key] + ","; // adds the product infomation and a "," to the end of the save_string
                    }
                    else
                    {
                        save_string = save_string + product[key]; // adds the product infomation to the end of the save_string
                    }
                    iteration_attributes++; // incressess iteration_attributes by 1
                }

                if (iteration_product < (LoadedLists[listName].Count - 1)) //checks if iteration_product is less then the amount of stuff in the list (that is name is passed though) minus one 
                    save_string = save_string + Environment.NewLine; // adds a new line to the end of the save_string
                iteration_product++; // incresses iteration_product by 1

            }

            StreamWriter loadedFile = new StreamWriter(fileName); // loads the file with the name fileName in write mode
            loadedFile.Write(save_string); // writes all lines in the save_string to the file
            loadedFile.Close(); // closes the file
        }

        /// <summary>
        /// Clears all items from the cart and updates the cart display.
        /// Empties the `cart` list and then calls `UpdateCart` to refresh
        /// the cart view and associated controls.
        /// </summary>
        public void ClearCart()
        {
            cart.Clear();
            UpdateCart();
        }

        /// <summary>
        /// Handles the selection change event for `sales_list_select`.
        /// If an item is selected, updates the wait list and product view with products from "product_list"
        /// and synchronizes the selection with `customer_list_select`.
        /// If no item is selected, clears the wait list.
        /// </summary>
        private void sales_list_select_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sales_list_select.SelectedItem != null)
            {
                UpdateWaitList(sales_list_select.SelectedItem.ToString());
                UpdateProductView(LoadedLists[sales_list_select.SelectedItem.ToString()]);
                customer_list_select.SelectedItem = sales_list_select.SelectedItem;
            }
            else
            {
                wait_list.Items.Clear();
            }
        }

        /// <summary>
        /// Handles the selection change event for `customer_list_select`.
        /// If an item is selected in `sales_list_select`, updates the wait list and product view with products from "product_list"
        /// and synchronizes the selection with `sales_list_select`.
        /// If no item is selected, clears the wait list.
        /// </summary>
        private void customer_list_select_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (customer_list_select.SelectedItem != null)
            {
                UpdateWaitList(customer_list_select.SelectedItem.ToString());
                UpdateProductView(LoadedLists[customer_list_select.SelectedItem.ToString()]);
                sales_list_select.SelectedItem = customer_list_select.SelectedItem;
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


            List<Dictionary<string, dynamic>> product_list = new List<Dictionary<string, dynamic>>(); 
            Dictionary<string, dynamic> new_product = new Dictionary<string, dynamic>(); // creates a Dictionary for new product infomation to be added to

            string selctedList;

            if ((input_product_id.Text == "") || (input_product_name.Text == "") || (input_product_price.Text == "") || (input_product_quantity.Text == "")) // checks if any fields are empty
            {
                MessageBox.Show("Error, Text box empty", "Error", MessageBoxButton.OK); // tell the user there is an empty field 
                return; //ends the function
            }

            if (!LoadedLists.ContainsKey("new_products")) // checks if new_products is in the list 
            {
                LoadedLists.Add("new_products", product_list); // creates new_products if it doesn't
                UpdateListSelecter();
            }

            foreach(string key in LoadedLists.Keys)
            {
                Trace.WriteLine(key);
            }


            if (sales_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                sales_list_select.SelectedItem = "new_products"; // sets selectedList to the list new_products
            }

            Trace.Write(sales_list_select.SelectedItem.ToString());
            selctedList = sales_list_select.SelectedItem.ToString(); // sets selectedList to the list the the user has selected in the list selection listbox

            product_list = LoadedLists[selctedList]; // sets product_list to the selected list


            foreach (Dictionary<string, dynamic> product in product_list) // loops though all the products that are in product_list
            {
                if (product["id"] == input_product_id.Text) // checks if the product id that the user has enterd is already in the list 
                {
                    MessageBox.Show("Error, Product ID already in list", "Error", MessageBoxButton.OK); // tells user that the id alread in the list
                    return ; // ends the function
                }
            }

            new_product.Add("id", input_product_id.Text); // stores users input in to the Dictionary with the key "id"

            new_product.Add("name", input_product_name.Text); // stores users input in to the Dictionary with the key "name"

            try { new_product.Add("price", Convert.ToDouble(input_product_price.Text)); } // trys to convert the users input into a double and store it to the Dictionary with the key "price"
            catch (Exception) // if an error occurrs in the convertion 
            {
                MessageBox.Show("Error, please enter a Double (decimal) in the price box", "Error", MessageBoxButton.OK); // tells the user and error has occurred 
                return; //ends the function
            }

            try { new_product.Add("quantity", Convert.ToInt32(input_product_quantity.Text)); } // trys to convert the users input into a int and store it to the Dictionary with the key "quantity"
            catch (Exception) // if an error occurrs in the convertion 
            {
                MessageBox.Show("Error, please enter a Interger (Whole number) in the quantity box", "Error", MessageBoxButton.OK); // tells the user and error has occurred 
                return; // ends the funtion
            }

            product_list.Add(new_product); // add the new product to the product list 
            LoadedLists[selctedList] = product_list; // saves the list to the big list 
            UpdateWaitList(selctedList); // updates the waitList listbox to show the new product
            UpdateListSelecter(); // updates the list selector listbox to show the new list if there is one created 
            sales_list_select.SelectedItem = selctedList; // selects the new list if there is one created 

            //sets all user input fields to blank
            input_product_id.Text = ""; 
            input_product_name.Text = "";
            input_product_price.Text = "";
            input_product_quantity.Text = "";
            
        }

        //part 2 
        /// <summary>
        /// checks if the user has selected a item.
        /// if they haven't it will clear the whole list.
        /// other wise it'll just clear the item 
        /// </summary>
        private void clear_list_Click(object sender, RoutedEventArgs e)
        {
            if (sales_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK);
                return; // ends the function 
            }
            if (wait_list.SelectedItem == null) //checks if the user has selected a item
            {
                LoadedLists[sales_list_select.SelectedItem.ToString()].Clear();  //if they haven't it will clear the whole list
                UpdateWaitList(sales_list_select.SelectedItem.ToString()); // updates the waitlist listbox
                return; // ends the function
            }

            foreach (string item in wait_list.SelectedItems) // loops though all items that are selected
            {
                LoadedLists[sales_list_select.SelectedItem.ToString()].RemoveAt(wait_list.Items.IndexOf(item)); // removes the item
            }
            UpdateWaitList(sales_list_select.SelectedItem.ToString()); // updates the waitlist listbox
        }

        //part 3
        private void save_file_Click(object sender, RoutedEventArgs e)
        {
            SaveFile("ProductFile.txt", sales_list_select.SelectedItem.ToString());
        }

        //part 4
        public void load_file_Click(object sender, RoutedEventArgs e)
        {
            LoadFile("ProductFile.txt");
        }


        //part B
        //part 1
        private void search_button_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK); // tells the user to select a list
                return; //ends the function
            }

            bool Item_found = false;
            product_view.Items.Clear(); // clears the product_view listbox
            string search_item = input_search_product.Text; //sets search_item to that the user inputs in the search box
            searchResualtsList.Clear(); // clears the searchResualtsList

            foreach (Dictionary<string, dynamic> product in LoadedLists[sales_list_select.SelectedItem.ToString()]) // loops though all products in the selected list from the selected
            {  
                if (product["name"].Contains(search_item) || product["id"].Contains(search_item)) // looks though the products id and name to see it they have the search term
                {
                    searchResualtsList.Add(product); // adds the the found item to the search resualts 
                    Item_found = true; 
                }
            }
            if (!Item_found) // checks if the Item_found = false
            {
                MessageBox.Show("Unable to find item", "Not Found", MessageBoxButton.OK); // sends a message to user that there search didnt find anything
            }
            else
            {
                UpdateProductView(searchResualtsList); // updates the product view list box
            }
        }

        //part 4
        private void place_order_Click(object sender, RoutedEventArgs e)
        {
            SelectMenu("checkout_page");
        }

        //part 2
        private void view_all_products_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.Items.Count == 0) // checks if the list selection listbox is empty
            {
                LoadFile("ProductFile.txt"); // loads the ProductFile.txt file
                customer_list_select.SelectedItem = "product_list"; // updates the selection listbox selected item to "product_list"
            }
            if (customer_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK); // tells the user to select a list
                return; //ends the function
            }
            UpdateProductView(LoadedLists[sales_list_select.SelectedItem.ToString()]); // updates the product view list box
        }

        //part 3
        private void add_to_cart_Click(object sender, RoutedEventArgs e)
        {
            if (customer_list_select.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK); // tells the user to select a list
                return; //ends the function
            }

            if (product_view.SelectedItem == null) // checks if the user has selected an item 
            {
                return; // ends the function
            }

            int add_ammount = 0; 

            if (cart_add_amount.Text == "") // checks if the quantity the user wants to add is blank
            {
                cart_add_amount.Text = "1"; // sets the quantity the user wants to add to 1
            }

            try { add_ammount = Convert.ToInt32(cart_add_amount.Text); } // trys to convert the quantity the user wants to add to and int
            catch (Exception) // if an error occurrs in the convertion
            {
                MessageBox.Show("Error, please enter a Interger (Whole number) in the box", "Error", MessageBoxButton.OK); // tells the user to enter an int 
                return; // ends the function 
            }

            List<Dictionary<string, dynamic>> target_list = new List<Dictionary<string, dynamic>>();

            if (searchResualtsList.Count() != 0) // if an item has been searched for
            {
                target_list = searchResualtsList; //pull from search resualts 
            }
            else
            {
                target_list = LoadedLists[sales_list_select.SelectedItem.ToString()]; //pull from the selected product list
            }

            Dictionary<string, dynamic> product = target_list[product_view.SelectedIndex]; 

            if (add_ammount > product["quantity"]) // checks if the amout the user wants to add is greater then the ammount avalible
            {
                MessageBox.Show("The amount you want to add is greater then the avalible ammount", "Limit reached", MessageBoxButton.OK); // tells the user the amount the want to add is to great
                return; // ends the function
            }

            bool item_not_found = true;
            foreach (Dictionary<string, dynamic> cart_item in cart) // loops though every item in the cart
            {
                if (cart_item["id"] != product["id"]) //finds item in cart
                {
                    continue; // goes to the next iteration of the loop
                }

                item_not_found = false;

                if ((add_ammount + cart_item["quantity"]) > product["quantity"]) // checks if the amount the user wants to add plus the amount in the cart is greater then the amount avalible
                {
                    MessageBox.Show("The amount you want to add is greater then the avalible ammount", "Limit reached", MessageBoxButton.OK); // tells the user the amount the want to add is to great
                    return; // ends the function
                }

                if (cart_item["quantity"] != product["quantity"]) // checks if the quantity of the cart item is greater then the avalible amount
                {
                    cart_item["quantity"] += add_ammount; // adds the amout that the yser wants to add
                    break; // ends the loop
                }

                else
                {
                    MessageBox.Show("Limit reached", "Limit reached", MessageBoxButton.OK); // tells the user that there is no more of that item avalible
                    break; // ends the loop
                }
            }

            if (item_not_found)
            {
                Dictionary<string, dynamic> product_info = new Dictionary<string, dynamic>(product); // creates a copy of the product 
                product_info["quantity"] = add_ammount;  // changes the copys quantity to the amout the want to add 
                cart.Add(product_info); // adds the copy to the cart
            }
            UpdateCart(); // updates the cart listbox

        }

        private void remove_from_cart_Click(object sender, RoutedEventArgs e)
        {
            if (cart_list_box.SelectedItem == null) // chesck if there is an list selected in the list selection listbox
            {
                MessageBox.Show("please Select a list in the box on the top right", "No list", MessageBoxButton.OK); // tells the user to select a list
                return; //ends the function
            }
            cart.RemoveAt(cart_list_box.SelectedIndex); // removes the selected item from the cart
            UpdateCart(); // updates the cart list box
        }

        private void clear_cart_Click(object sender, RoutedEventArgs e)
        {
            ClearCart();
        }


        //part C
        //part 1
        private void btn_checkout_Click(object sender, RoutedEventArgs e)
        {
            //checks if the text boxes are empty
            bool empty_box = false;
            if (checkout_name_box.Text == "") empty_box = true;
            if (checkout_contact_number_box.Text == "") empty_box = true;
            if (checkout_email_box.Text == "") empty_box = true;
            if (checkout_address_box.Text == "") empty_box = true;
            if (empty_box) //checks if the text boxes are empty
            {
                MessageBox.Show("Error, Text box empty", "Error", MessageBoxButton.OK); // send a message saying the boxes are empty
                return; //ends function
            }
            MessageBox.Show("Thank You", "Thank You", MessageBoxButton.OK); // sends a thank you message



            List<Dictionary<string, dynamic>> updatedList = LoadedLists[sales_list_select.SelectedItem.ToString()]; 

            foreach (Dictionary<string, dynamic> orderedProduct in cart) //loops for the amount of products in the cart
            {
                string idToFind = orderedProduct["id"];
                foreach (Dictionary<string, dynamic> savedProduct in LoadedLists[sales_list_select.SelectedItem.ToString()]) // loops for the amount of products in the product list
                {
                    if (idToFind != savedProduct["id"]) // checks if the cart products id is the same as the products from the product list
                    {
                        continue; // goes to next iteration of the loop
                    }

                    savedProduct["quantity"] = savedProduct["quantity"] - orderedProduct["quantity"]; // removes the amount of products the user wants from the 
                    break;

                }

            }

            LoadedLists[sales_list_select.SelectedItem.ToString()] = updatedList; 
            SaveFile("productFile.txt", sales_list_select.SelectedItem.ToString());

        }

        private void btn_edit_cart_Click(object sender, RoutedEventArgs e)
        {
            SelectMenu("customer_page");
        }


    }
}