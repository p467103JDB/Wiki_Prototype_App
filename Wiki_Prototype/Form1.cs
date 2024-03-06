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
        public Form1()
        {
            InitializeComponent();
            InitializeGlobalArray(listView_Array);
        }

        // Initialize globals
        static readonly int Col = 4;    // AFAIK, capital first letters are used for public global variables - Pascal Case
        static readonly int Row = 12;   // Also, static is a changeable data type which is good. but readonly makes it similar to a constant and these are supposed to be the maximum amount.
        static int CurrentTotal = 0;    // this keeps track of the actual number of items in the list
        string[,] GlobalArray = new string[Row, Col];

        // This can function as a reset
        private void InitializeGlobalArray(ListView listView_Array)
        {
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    GlobalArray[i, j] = null; // they're all assigned as string but left unassisnged. it works as seen in the below method.
                }
            }
            textBox_Name.Focus();
        }

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
                        for (int j = 0; j < CurrentTotal - 1; j++) // -1 otherwise we out of bounds bruh
                        {
                            if (string.Compare(GlobalArray[j, 0], GlobalArray[j + 1, 0]) > 0) // String compare, swap in ascending order - not worried about duplicates just yet, i fully expect them to show up though
                            {
                                // using the swapsie tuple decontructor again :^) 
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

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (CurrentTotal < Row)
            {
                try
                {
                    // Check if the textboxes are empty
                    if (!string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
                    {
                        // we know the current total and positions oavailiable and we also know we can't go past 12. 
                        GlobalArray[CurrentTotal, 0] = textBox_Name.Text;
                        GlobalArray[CurrentTotal, 1] = textBox_Category.Text;
                        GlobalArray[CurrentTotal, 2] = textBox_Struct.Text;
                        GlobalArray[CurrentTotal, 3] = textBox_Definition.Text;
                        toolStripStatusLabel1.Text = "Item succesfully added to list ar index: " + CurrentTotal;
                        CurrentTotal++;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = "Item could not be added, missing criteria: Textboxes cannot be null or empty.";
                    }
                }

                catch (Exception ex)
                {
                    toolStripStatusLabel1.Text = ex.Message;
                }
                InitializeListView(listView_Array);
                button_Reset_Click(sender, e);
            }

            else
            {
                toolStripStatusLabel1.Text = "Cannot add new item. Limit of  " + Row + " reached.";
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (listView_Array.SelectedIndices.Count > 0) // straight forward, if there isnt any items dont delete lol
            {
                int selectedInde = listView_Array.SelectedIndices[0]; // get selected

                // Prompt user first before delete
                DialogResult result = MessageBox.Show("Do you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's choice
                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < Col; i++)
                    {
                        GlobalArray[selectedInde, i] = null;
                    }
                    CurrentTotal--;
                    InitializeListView(listView_Array);
                    button_Reset_Click(sender, e);
                    toolStripStatusLabel1.Text = "Succesfully deleted item at index: " + selectedInde;
                }
                else
                {
                    toolStripStatusLabel1.Text = "User chose not to delete.";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Cannot delete item, no item or data availiable.";
            }
        }

        private void button_Edit_Click(object sender, EventArgs e)
        {
            if (listView_Array.SelectedIndices.Count > 0 && !string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
            {
                int selectedIndex = listView_Array.SelectedIndices[0];
                GlobalArray[selectedIndex, 0] = textBox_Name.Text;
                GlobalArray[selectedIndex, 1] = textBox_Category.Text;
                GlobalArray[selectedIndex, 2] = textBox_Struct.Text;
                GlobalArray[selectedIndex, 3] = textBox_Definition.Text;
                InitializeListView(listView_Array);
                button_Reset_Click(sender, e);
                toolStripStatusLabel1.Text = "Succesfully edited item at index: " + selectedIndex;
            }
            else if(listView_Array.SelectedIndices.Count == 0)
            {
                toolStripStatusLabel1.Text = "Could not edit item as there is no item.";
            }
            else
            {
                toolStripStatusLabel1.Text = "Could not edit item as textboxes for data are empty.";
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            textBox_Name.Text = string.Empty;
            textBox_Category.Text = string.Empty;
            textBox_Struct.Text = string.Empty;
            textBox_Definition.Text = string.Empty;
            textBox_Name.Focus();
            toolStripStatusLabel1.Text = "Text boxes cleared.";
        }

        private void button_Save_Click(object sender, EventArgs e)
        {

        }

        private void button_Load_Click(object sender, EventArgs e)
        {

        }

        private void listView_Array_Click(object sender, EventArgs e)
        {
            if (listView_Array.SelectedIndices.Count > 0)
            {
                int selectedIndex = listView_Array.SelectedIndices[0];

                textBox_Name.Text = GlobalArray[selectedIndex, 0];
                textBox_Category.Text = GlobalArray[selectedIndex, 1];
                textBox_Struct.Text = GlobalArray[selectedIndex, 2];
                textBox_Definition.Text = GlobalArray[selectedIndex, 3];
                textBox_Name.Focus();
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            // perform binary search
            string search = textBox_Search.Text;
            if (!string.IsNullOrEmpty(search) && CurrentTotal > 1)
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
                        if (midselection == search) // Found - successfull
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

                catch (Exception exc)
                {
                    toolStripStatusLabel1.Text = exc.Message;
                }

                if (foundIndex != -1)
                {
                    toolStripStatusLabel1.Text = $"Binary search - Found at index: {foundIndex}";
                }
                else
                {
                    toolStripStatusLabel1.Text = "Search item not found.";
                }
            }

            // Cant search with less than 2
            else if (CurrentTotal < 2)
            {
                toolStripStatusLabel1.Text = "Cannot binary search with less than 2 items in list.";
            }

            // Can't search with empty text box
            else
            {
                toolStripStatusLabel1.Text = "Cannot search with empty text box.";
            }
        }
    }
}
