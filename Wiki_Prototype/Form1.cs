using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wiki_Prototype
{
    public partial class Form1 : Form
    {
        // P467103 - JACK DU BOULAY
        // WIKI APP PROGRAM
        public Form1()
        {
            InitializeComponent();
        }

        // Question 9.1 - Global array + static variables
        // Initialize globals   
        static readonly int Col = 4;                    // AFAIK, capital first letters are used for public global variables - Pascal Case
        static readonly int Row = 12;                   // Also, static is a changeable data type which is good. but readonly makes it similar to a constant and this is supposed to be the maximum amount.
        static int CurrentTotal = 0;                    // this keeps track of the actual number of items in the list
        string[,] GlobalArray = new string[Row, Col];   // GlobalArray is the 2D array

        // Question 9.8 - Display list view
        private void InitializeListView(ListView listView_Array)
        {
            // Why am i doing this? Just to make sure it works.
            listView_Array.Items.Clear();
            //start array sorting method first. then add...
            InitialzeBubbleSortMethod();
            for (int i = 0; i < CurrentTotal; i++)
            {
                ListViewItem check_item = listView_Array.Items.Add(GlobalArray[i, 0]);
                for (int j = 1; j < Col; j++)   //j has to be 1 because i'd be adding the name again.... XD my bad 
                {
                    // this might not even be necessary, it's basically adding the struct and definition despite it not being visible in the listview :S
                    check_item.SubItems.Add(GlobalArray[i, j]);
                }
            }
        }

        // Question 9.6 - Bubble sort method
        private void InitialzeBubbleSortMethod()
        {
            try
            {
                if (CurrentTotal > 1)
                {
                    bool isSwapped;
                    do
                    {
                        isSwapped = false; // every loop will set to false if it's false then break out of loop if all is done
                        for (int j = 0; j < CurrentTotal - 1; j++) // current total -1 otherwise we out of bounds bruh
                        {
                            if (string.Compare(GlobalArray[j, 0], GlobalArray[j + 1, 0]) > 0) // String compare, swap in ascending order - not worried about duplicates just yet, i fully expect them to show up in part 2 though
                            {
                                // using the swapsie tuple decontructor again coz it's cooler than using a new dummy temp var :^) 
                                (GlobalArray[j, 0], GlobalArray[j + 1, 0]) = (GlobalArray[j + 1, 0], GlobalArray[j, 0]);    // Swap name
                                (GlobalArray[j, 1], GlobalArray[j + 1, 1]) = (GlobalArray[j + 1, 1], GlobalArray[j, 1]);    // Swap category
                                (GlobalArray[j, 2], GlobalArray[j + 1, 2]) = (GlobalArray[j + 1, 2], GlobalArray[j, 2]);    // Swap structure
                                (GlobalArray[j, 3], GlobalArray[j + 1, 3]) = (GlobalArray[j + 1, 3], GlobalArray[j, 3]);    // Swap Definition
                                isSwapped = true;
                            }
                        }
                    }
                    while (isSwapped);
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.Message;
            }
        }

        // Question 9.2 - Add button
        private void button_Add_Click(object sender, EventArgs e)
        {
            if (CurrentTotal < Row)
            {
                bool isAdded = false;
                // Check if the textboxes are empty
                if (!string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
                {
                    // we know the current total and positions availiable and we also know we can't go past 12. 
                    GlobalArray[CurrentTotal, 0] = textBox_Name.Text;
                    GlobalArray[CurrentTotal, 1] = textBox_Category.Text;
                    GlobalArray[CurrentTotal, 2] = textBox_Struct.Text;
                    GlobalArray[CurrentTotal, 3] = textBox_Definition.Text;
                    isAdded = true;
                    CurrentTotal++;
                }
                // Criteria met and added
                if (isAdded)
                {
                    InitializeListView(listView_Array);
                    button_Reset_Click(sender, e);
                    toolStripStatusLabel1.Text = "Item succesfully added to list ar index: " + CurrentTotal;
                }
                // Criteria not met
                else
                {
                    toolStripStatusLabel1.Text = "Item could not be added, missing criteria: Textboxes cannot be null or empty.";
                }
            }
            // Max limit reached
            else
            {
                toolStripStatusLabel1.Text = "Cannot add new item. Limit of  " + Row + " reached.";
            }
        }

        // Question 9.4 - Delete button
        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listView_Array.SelectedIndices.Count > 0) // straight forward, if there isnt any items dont delete lol
            {
                int selectedIndex = listView_Array.SelectedIndices[0]; // get selected

                // Prompt user first with messagebox before delete
                DialogResult result = MessageBox.Show("Do you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's choice
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < Col; i++)
                    {
                        GlobalArray[selectedIndex, i] = null;
                    }
                    CurrentTotal--;
                    InitializeListView(listView_Array);
                    button_Reset_Click(sender, e);
                    toolStripStatusLabel1.Text = "Succesfully deleted item at index: " + selectedIndex;
                }
                else
                {
                    toolStripStatusLabel1.Text = "User chose not to delete.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Cannot delete item, no item selected and no data availiable.";
            }
        }

        // Question 9.3 - Edit button
        private void button_Edit_Click(object sender, EventArgs e)
        {
            // Check for criteria text boxes - They should not be empty
            if (listView_Array.SelectedIndices.Count > 0 && !string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
            {
                int selectedIndex = listView_Array.SelectedIndices[0];  // there can only be one as multiselect isnt enabled
                GlobalArray[selectedIndex, 0] = textBox_Name.Text;      // set text in array to the textbox text
                GlobalArray[selectedIndex, 1] = textBox_Category.Text;  
                GlobalArray[selectedIndex, 2] = textBox_Struct.Text;
                GlobalArray[selectedIndex, 3] = textBox_Definition.Text;

                // run through methods to update list and then clear boxes once done
                InitializeListView(listView_Array);
                button_Reset_Click(sender, e);
                toolStripStatusLabel1.Text = "Succesfully edited item at index: " + selectedIndex;
            }
            else if(listView_Array.SelectedIndices.Count == 0)
            {
                toolStripStatusLabel1.Text = "Could not edit item as there is no item selected and there isnt one avaliable.";
            }
            else
            {
                toolStripStatusLabel1.Text = "Could not edit item as textboxes for data are empty.";
            }
        }

        // Question 9.5 - Clear button
        private void button_Reset_Click(object sender, EventArgs e)
        {
            // Clear all textboxes below and focus on name
            textBox_Name.Text = string.Empty;
            textBox_Category.Text = string.Empty;
            textBox_Struct.Text = string.Empty;
            textBox_Definition.Text = string.Empty;
            textBox_Name.Focus();
            toolStripStatusLabel1.Text = "Text boxes cleared.";
        }

        // Question 9.10 - Save button
        private void button_Save_Click(object sender, EventArgs e)
        {
            /////// TODO ///////
        }

        // Question 9.11 - Load button
        private void button_Load_Click(object sender, EventArgs e)
        {
            /////// TODO ///////
        }
        // Question 9.9 - Display Definition 
        private void listView_Array_Click(object sender, EventArgs e)
        {
            // If user clicks on the left cell
            if (listView_Array.SelectedIndices.Count > 0)
            {
                int selectedIndex = listView_Array.SelectedIndices[0];

                // Present the array data for that index
                textBox_Name.Text = GlobalArray[selectedIndex, 0];
                textBox_Category.Text = GlobalArray[selectedIndex, 1];
                textBox_Struct.Text = GlobalArray[selectedIndex, 2];
                textBox_Definition.Text = GlobalArray[selectedIndex, 3];
                textBox_Name.Focus();
            }
        }

        // Question 9.7 - Binary Search button
        private void button_Search_Click(object sender, EventArgs e)
        {
            // perform binary search
            string search = textBox_Search.Text;
            if (!string.IsNullOrEmpty(search) && CurrentTotal > 0)
            {
                int foundIndex = -1;
                int left = 0;
                int right = CurrentTotal;
                try
                {
                    while (left <= right) 
                    {
                        // Find Mid
                        int mid = left + (right - left) / 2;
                        string midselection = GlobalArray[mid, 0]; // get name of the indexed data point
                        // Found - successfull
                        if (midselection == search) 
                        {
                            foundIndex = mid;
                            break;
                        }

                        // Going Left
                        else if (string.Compare(midselection, search, StringComparison.OrdinalIgnoreCase) > 0)
                        {
                            // Adjusting the right bound
                            right = mid - 1;
                        }

                        // Going Right
                        else
                        {
                            // Adjusting the left bound
                            left = mid + 1;
                        }
                    }
                }
                catch (Exception exc) // what bronk? only used for testing and getting out of bounds... sigh
                {
                    toolStripStatusLabel1.Text = exc.Message;
                }

                if (foundIndex != -1)
                {
                    toolStripStatusLabel1.Text = "Binary search - Found at index: " + foundIndex;
                }
                else
                {
                    toolStripStatusLabel1.Text = "Search item not found.";
                }
            }
            // Can't search with empty text box
            else
            {
                toolStripStatusLabel1.Text = "Cannot search with empty text box.";
            }
        }
    }
}
