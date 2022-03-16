using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace kolyandes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           // textBox1.Text = "1234asda";
           // textBox2.Text = "12345678";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DES desAlg = DES.Create())
            {
               
                desAlg.Key = Encoding.ASCII.GetBytes(textBox1.Text);
                desAlg.IV = Encoding.ASCII.GetBytes(textBox2.Text);
                

                textBox4.Text = Convert.ToBase64String(EncryptTextToMemory(textBox3.Text, desAlg.Key, desAlg.IV));
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (DES myDes = DES.Create())
            {
                myDes.Mode = CipherMode.ECB;

                // Encrypt the string to an base64 string
                try
                {
                    myDes.Key = Encoding.ASCII.GetBytes(textBox1.Text);
                    myDes.IV = Encoding.ASCII.GetBytes(textBox2.Text);
                    byte[] asd = Convert.FromBase64String(textBox4.Text);
                    textBox3.Text = DecryptTextFromMemory(asd, myDes.Key, myDes.IV);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        public static byte[] EncryptTextToMemory(string Data, byte[] Key, byte[] IV)
        {
            
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the MemoryStream
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    DESalg.CreateEncryptor(Key, IV),
                    CryptoStreamMode.Write);

                // Convert the passed string to a byte array.
                byte[] toEncrypt = new ASCIIEncoding().GetBytes(Data);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(toEncrypt, 0, toEncrypt.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the
                // MemoryStream that holds the
                // encrypted data.
                byte[] ret = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                // Return the encrypted buffer.
                return ret;
            
        }


        public static string DecryptTextFromMemory(byte[] Data, byte[] Key, byte[] IV)
        {
           
          
                // Create a new MemoryStream using the passed
                // array of encrypted data.
                MemoryStream msDecrypt = new MemoryStream(Data);

                // Create a new DES object.
                DES DESalg = DES.Create();

                // Create a CryptoStream using the MemoryStream
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    DESalg.CreateDecryptor(Key, IV),
                    CryptoStreamMode.Read);

                // Create buffer to hold the decrypted data.
                byte[] fromEncrypt = new byte[Data.Length];

                // Read the decrypted data out of the crypto stream
                // and place it into the temporary buffer.
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);

                //Convert the buffer into a string and return it.
                return new ASCIIEncoding().GetString(fromEncrypt);
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pavadinimas = "Sifruotas.txt";

            File.WriteAllText(pavadinimas, textBox4.Text);

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string FileName = "Sifruotas.txt";
            // Create or open the specified file. 
            String asd = File.ReadAllText(FileName);
            using (DES Desas = DES.Create())
            {
                Desas.Key = Encoding.ASCII.GetBytes(textBox1.Text);
                Desas.IV = Encoding.ASCII.GetBytes(textBox2.Text);
                byte[] enc = Convert.FromBase64String(asd);

                textBox3.Text = DecryptTextFromMemory(enc, Desas.Key, Desas.IV);
            }



            
        }
    }

}

