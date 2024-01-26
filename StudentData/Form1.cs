using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_list(cbb_province, "province.txt");
            load_list(cbb_district, "district.txt");
            load_list(cbb_commune, "commune.txt");
            load_list(cbb_village, "village.txt");
            load_list(cbb_gender, "gender.txt");
            load_list(cbb_position, "position.txt");
            load_list(cbb_department, "department.txt");
            load_list(dgv_list_emp);
        }

        private void load_list(ComboBox cbb, string data_list) {
            string filePath = $"data/{data_list}";
            using (StreamReader reader = new StreamReader(filePath)) {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    cbb.Items.Add(line);
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            string student_code = txtb_student_code.Text;
            string first_name = txtb_first_name.Text;
            string last_name = txtb_last_name.Text;
            string gender = cbb_gender.Text;
            string dob = dtp_dob.Text;
            string salary = txtb_salary.Text;
            string position = cbb_position.Text;
            string department = cbb_department.Text;
            string phone = txtb_phone.Text;
            string ID = txtb_id.Text;
            string home = txtb_home.Text;
            string village = cbb_village.Text;
            string district = cbb_district.Text;
            string commune = cbb_commune.Text;
            string province = cbb_province.Text;

            string address = $"{home},{village},{district},{commune},{province}";
            string name = $"{first_name} {last_name}";

            dgv_list_emp.Rows.Add(dgv_list_emp.RowCount, student_code, name, gender, dob, position, department, salary, phone, address, "Edit, Remove");
           
            string img_image = save_img();
            string data = $"{student_code}:{first_name}:{last_name}:{gender}:{dob}:" + $"{salary}:{position}:{department}:{phone}:" +$"{ID}:{home}:{village}:{district}:{commune}:" +$"{province}:{img_image}";

            save_data(data);
        }

        private void load_list(DataGridView data_list)
        {
            string filePath = "data/emp_list.txt";
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] ldata = line.Split(':');

                    // Validate that the line contains enough elements
                    if (ldata.Length >= 15)
                    {
                        string code = ldata[0];
                        string first_name = ldata[1];
                        string last_name = ldata[2];
                        string gender = ldata[3];
                        string dob = ldata[4];
                        string salary = ldata[5];
                        string position = ldata[6];
                        string department = ldata[7];
                        string phone = ldata[8];
                        string ID = ldata[9];
                        string home = ldata[10];
                        string village = ldata[11];
                        string district = ldata[12];
                        string commune = ldata[13];
                        string province = ldata[14];

                        string address = $"{home},{village},{district},{commune},{province}";
                        string name = $"{first_name} {last_name}";

                        data_list.Rows.Add(data_list.RowCount, code, name, gender, dob, salary, position, department, phone, address, "Edit, Remove");
                    }
                    else
                    {
                        Console.WriteLine("Invalid data format: " + line);
                    }
                }
            }
        }

        private void save_data(string data)
        {
            using (StreamWriter writer = new StreamWriter("data/emp_list.txt", true, Encoding.UTF8))
            {
                writer.WriteLine(data);
            }
        }

        private string save_img()
        {
            string img_name = "";
            if(pdb_image.Image != null)
            {
                Guid img = Guid.NewGuid();
                img_name = $"{img}.jpeg";
                string filePath = Path.Combine(Application.StartupPath, "img", img_name);
                pdb_image.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                MessageBox.Show("No image found in the PictureBox.");
            }
            return img_name;
        }

        private void open_img()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select an Image File";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp|All Files (*.*)|*.*";
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFileName = openFileDialog.FileName;
                    lbl_img.Text = selectedFileName;
                    try
                    {
                        Image selectedImage = Image.FromFile(selectedFileName);
                        pdb_image.Image = selectedImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            } 
        }

        private void btn_open_img_Click(object sender, EventArgs e)
        {
            open_img();
        }
    }
}
