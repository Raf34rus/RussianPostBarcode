using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RBBarcode;

namespace UseLibrary
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image =  PostBarcode.Print2of5Interleaved(textBox1.Text);//14211716007389
            pictureBox5.Image =  PostBarcode.PrintQRCode(textBox1.Text);//14211716007389
            Bitmap gggg = PostBarcode.PrintQRCode(textBox1.Text);
            //pictureBox5.Image.Save("test128.bmp");           
            gggg.Save("test_QR.bmp");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox4.Image = PostBarcode.PrintPostcode(textBox2.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
