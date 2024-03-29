﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// JACK DU BOULAY
// P467103
// 13/03/2024
// WIKI APP PROGRAM
// Cluster - C Sharp Two - Assessmnet 1 

namespace Wiki_Prototype
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeListView(ListView_Array);
            toolStripStatusLabel1.Text = "Program opened successfully.";
        }

        // Question 9.1 - Global array + static variables
        // Initialize globals   
        static readonly int Col = 4;                    // AFAIK, capital first letters are used for public global variables - Pascal Case
        static readonly int Row = 12;                   // Also, static is a changeable data type which is good. but readonly makes it similar to a constant and this is supposed to be the maximum amount.
        static int CurrentTotal = 0;                    // this keeps track of the actual number of items in the list
        string[,] GlobalArray = new string[Row, Col];   // GlobalArray is the 2D array

        private void InitializeGlobalArray() // wipes all array data
        {
            for (int i = 0; i < Row; i++)
            {

                for (int j = 0; j < Col; j++)
                {
                    GlobalArray[i, j] = null;
                }
            }
            CurrentTotal = 0;
        }

        // Question 9.8 - Display list view
        private void InitializeListView(ListView listView_Array)
        {
            listView_Array.Items.Clear();

            //start array sorting method first. then add...
            for (int i = CurrentTotal; i < Row; i++) // add tild squiggles - should start from 0 - so we should only see tilds in the cells
            {
                for (int j = 0; j < Col; j++)
                {
                    GlobalArray[i, j] = "~";
                }
            }

            InitialzeBubbleSortMethod();

            for (int i = 0; i < Row; i++)
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

                            if (string.CompareOrdinal(GlobalArray[j, 0], GlobalArray[j + 1, 0]) > 0 ) // Cheers Stewart! CompareOrdinal is a much better way of doing string comparisons in this assessment.
                            {
                                // using the swapsie tuple decontructor again coz it's cooler than using a new dummy temp var :^)  - this could be shorter***
                                for (int k = 0; k < Col; k++) // this will swap name cat struct and definition
                                {
                                    (GlobalArray[j, k], GlobalArray[j + 1, k]) = (GlobalArray[j + 1, k], GlobalArray[j, k]);
                                }
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
        private void Button_Add_Click(object sender, EventArgs e)
        {
            if (CurrentTotal < Row)
            {
                bool isAdded = false;
                // Check if the textboxes are empty
                if (!string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
                {
                    // we know the current total and positions availiable and we also know we can't go past 12. 
                    GlobalArray[CurrentTotal, 0] = textBox_Name.Text.ToUpper(); // <-- TEST REPORT***  - Fixed an issue where lowercase went below uppercase
                    GlobalArray[CurrentTotal, 1] = textBox_Category.Text;
                    GlobalArray[CurrentTotal, 2] = textBox_Struct.Text;
                    GlobalArray[CurrentTotal, 3] = textBox_Definition.Text;
                    isAdded = true;
                    CurrentTotal++;
                }
                // Criteria met and added
                if (isAdded)
                {
                    InitializeListView(ListView_Array);
                    Button_Reset_Click(sender, e);
                    toolStripStatusLabel1.Text = "Item succesfully added to list at index: " + (CurrentTotal - 1);
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
                toolStripStatusLabel1.Text = "Cannot add new item. Limit of " + Row + " reached.";
            }
        }

        // Question 9.4 - Delete button
        private void Button_Delete_Click(object sender, EventArgs e)
        {
            if (ListView_Array.SelectedIndices.Count > 0) // straight forward, if there isnt any items dont delete lol
            {
                int selectedIndex = ListView_Array.SelectedIndices[0]; // get selected
                string s = GlobalArray[selectedIndex, 0]; // THIS IS SO SCUFFED ***
                string t = "~";

                // Prompt user first with messagebox before delete
                DialogResult result = MessageBox.Show("Do you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Check the user's choice
                if(s == t)
                {
                    toolStripStatusLabel1.Text = "Cannot remove empty cell.";
                }

                else if (result == DialogResult.Yes)
                {
                    // UHHHH SIGH, im sure theres an easier way, this will basically swap the selected array index with the one at the end. this ensures that there arent empty cells of data in the array at the front.
                    for (int i = 0; i < Col; i++) // wipe the final index 
                    {
                        GlobalArray[selectedIndex, i] = "~";
                    }
                   
                    InitializeListView(ListView_Array);
                    Button_Reset_Click(sender, e);
                    CurrentTotal--;
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
        private void Button_Edit_Click(object sender, EventArgs e)
        {
            // Check for criteria text boxes - They should not be empty
            if (ListView_Array.SelectedIndices.Count > 0 && !string.IsNullOrEmpty(textBox_Name.Text) && !string.IsNullOrEmpty(textBox_Category.Text) && !string.IsNullOrEmpty(textBox_Struct.Text) && !string.IsNullOrEmpty(textBox_Definition.Text))
            {
                int selectedIndex = ListView_Array.SelectedIndices[0];  // there can only be one as multiselect isnt enabled
                GlobalArray[selectedIndex, 0] = textBox_Name.Text;      // set text in array to the textbox text
                GlobalArray[selectedIndex, 1] = textBox_Category.Text;  
                GlobalArray[selectedIndex, 2] = textBox_Struct.Text;
                GlobalArray[selectedIndex, 3] = textBox_Definition.Text;

                // run through methods to update list and then clear boxes once done
                InitializeListView(ListView_Array);
                Button_Reset_Click(sender, e);
                toolStripStatusLabel1.Text = "Succesfully edited item at index: " + selectedIndex;
            }
            else if(ListView_Array.SelectedIndices.Count == 0)
            {
                toolStripStatusLabel1.Text = "Could not edit item as there is no item selected.";
            }
            else
            {
                toolStripStatusLabel1.Text = "Could not edit item as textboxes for data are empty.";
            }
        }

        // Question 9.5 - Clear button
        private void Button_Reset_Click(object sender, EventArgs e)
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
        private void Button_Save_Click(object sender, EventArgs e)
        {
            // The example stew provided was helpful enough in learning content session 4 file i/o.
            // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.savefiledialog?view=windowsdesktop-7.0#examples // But heres what i actually needed to know.
            SaveFileDialog saveFileDialog = new SaveFileDialog          // Create a new instance of SaveFileDialog - found out this could be simplified 
            {
                InitialDirectory = Application.StartupPath,             // Default directory is the wiki_prototype/bin/debug
                FileName = "definitions",                               // Set the default filename - not hard coded, it can be changed by the user in the dialog window
                DefaultExt = ".dat",                                    // Extention will be set to .dat
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*"  // Make a filter to show only .dat files
            };                                                         

            // Show the SaveFileDialog and get the result
            DialogResult writer = saveFileDialog.ShowDialog();
            if (writer == DialogResult.OK)
            {
                BinaryWriter bW;
                try
                {
                    bW = new BinaryWriter(new FileStream(saveFileDialog.FileName, FileMode.Create)); // Bruh, it can't be appended. make new one every time
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nCannot append to file.");
                    return;
                }

                // Write to file
                try
                {
                    for (int i = 0; i < CurrentTotal; i++)
                    {
                        for (int j = 0; j < Col; j++)
                        {
                                bW.Write(GlobalArray[i, j]); // each string is automatically split. It doesn't matter to add commas or breaks
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nCannot write data to file.");
                    return;
                }
                bW.Close();
                toolStripStatusLabel1.Text = "Succesfully saved data to: " + saveFileDialog.FileName;
            }
            else
            {
                toolStripStatusLabel1.Text = "User chose NOT to save file.";
            }
        }

        // Question 9.11 - Load button
        private void Button_Load_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog instance - Can be simplified like so
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                DefaultExt = ".dat",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*"
            };
            // Show the OpenFileDialog and get the result
            DialogResult reader = openFileDialog.ShowDialog();

            // Open file
            if (reader == DialogResult.OK)
            {
                // CLEAR GLOBAL ARRAY
                InitializeGlobalArray();
                BinaryReader bR; // create new instance of binaryReader.
                try
                {
                    bR = new BinaryReader(new FileStream(openFileDialog.FileName, FileMode.Open));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\nCannot open file for reading");
                    return; // return and finish method
                }

                while (bR.BaseStream.Position != bR.BaseStream.Length)
                {
                    try
                    {
                        for (int i = 0; i < Row; i++)
                        {
                            if (bR.BaseStream.Position == bR.BaseStream.Length) // HAH! fixed the problem of reading an empty non existant string.
                            {
                                break;
                            }

                            // Simplified my logic in this load
                            for (int j = 0; j < Col; j++)
                            {
                                GlobalArray[i, j] = bR.ReadString();
                            }

                            CurrentTotal++; // new item has been added
                        }

                        InitializeListView(ListView_Array); // Update list with new content :)
                        toolStripStatusLabel1.Text = "Successfully loaded file from: " + openFileDialog.FileName;
                    }
                    catch (Exception fe)
                    {
                        MessageBox.Show(fe.Message + "\nCannot read data from file or EOF");
                        break;
                    }
                }
                bR.Close(); // Close binary reader 
            }
            else
            {
                toolStripStatusLabel1.Text = "User chose not to load a file.";
            }
        }
        // Question 9.9 - Display Definition 
        private void ListView_Array_Click(object sender, EventArgs e)
        {
            // If user clicks on the left cell
            if (ListView_Array.SelectedIndices.Count > 0)
            {
                int selectedIndex = ListView_Array.SelectedIndices[0];
                // Present the array data for that index
                textBox_Name.Text = GlobalArray[selectedIndex, 0];
                textBox_Category.Text = GlobalArray[selectedIndex, 1];
                textBox_Struct.Text = GlobalArray[selectedIndex, 2];
                textBox_Definition.Text = GlobalArray[selectedIndex, 3];
                textBox_Name.Focus();
            }
        }

        // Question 9.7 - Binary Search button
        private void Button_Search_Click(object sender, EventArgs e)
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
                        if (string.Compare(midselection, search, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            foundIndex = mid;
                            ListView_Array.Items[foundIndex].Selected = true;
                            ListView_Array_Click(sender, e);
                            break;
                        }

                        // Going Left
                        else if (string.Compare(midselection, search, StringComparison.OrdinalIgnoreCase) > 0) 
                        {
                            // Adjusting the right bound
                            right = mid - 1;
                        }

                        // Going Right
                        else if (string.Compare(midselection, search, StringComparison.OrdinalIgnoreCase) < 0)
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
