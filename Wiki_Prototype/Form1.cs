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
        // Initialize globals
        const int Col = 4; // AFAIK, capital first letters are used for public global variables - Pascal Case
        const int Row = 12;
        string[,] GlobalArray = new string[Row, Col];

        public Form1()
        {
            InitializeComponent();
            InitializeGlobalArray();
        }

        private void InitializeGlobalArray()
        {
            for (int i = 0; i < Row; i++)
            {

                for(int j = 0; j < Col; j++)
                {
                    GlobalArray[i, j] = "";
                }
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {

        }

        private void button_Delete_Click(object sender, EventArgs e)
        {

        }

        private void button_Edit_Click(object sender, EventArgs e)
        {

        }

        private void button_Reset_Click(object sender, EventArgs e)
        {

        }

        private void button_Save_Click(object sender, EventArgs e)
        {

        }

        private void button_Load_Click(object sender, EventArgs e)
        {

        }
    }
}
