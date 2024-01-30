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
        static string db = "data/emp_list.txt";

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
            string data_row = preparing_data();
            if(lbl_save_action.Text == "Save Mode")
            {
                save_data(data_row);
                lbl_save_action.Text = "Save Mode";
            }
            else if (lbl_save_action.Text == "Edit Mode")
            {
                emp_edit_info(data_row);
                dgv_list_emp.Rows.Clear();
                load_list(dgv_list_emp);
                lbl_save_action.Text = "Save Mode";
            }
        }

        private void load_list(DataGridView data_list)
        {
            //string filePath = "data/emp_list.txt";
            using (StreamReader reader = new StreamReader(db))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] ldata = line.ToString().Split(';');

                    // Validate that the line contains enough elements
                    if (ldata.Length >= 16)
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

                        data_list.Rows.Add(data_list.RowCount, code, name, gender, dob, salary, position, department, phone, address, "Edit", "Remove");
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

        private string preparing_data()
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
            string img_image = save_img();
            if(lbl_save_action.Text == "Save Mode")
            {
                dgv_list_emp.Rows.Add(dgv_list_emp.RowCount, student_code, name, gender, dob, position, department, salary, phone, address, "Edit", "Remove");
            }
            string row = $"{student_code};{first_name};{last_name};{gender};{dob};" + 
                $"{salary};{position};{department};{phone};" + 
                $"{ID};{home};{village};{district};{commune};" + 
                $"{province};{img_image}";

            return row;
        }

        private void selec_row_data_list(DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int row = e.RowIndex;
                string emp_code = dgv_list_emp.Rows[row].Cells[1].Value.ToString();
                string emp_row = find_emp_by_code(emp_code);
                List<string> data_rows = new List<string>(emp_row.Split(';'));
                if(emp_row != "")
                {
                    string img_path = Application.StartupPath + $"\\img\\{data_rows[15]}";
                    pdb_image.Image = System.Drawing.Image.FromFile(img_path);
                    txtb_student_code.Text = data_rows[0];
                    txtb_first_name.Text = data_rows[1];
                    txtb_last_name.Text = data_rows[2];
                    cbb_gender.Text = data_rows[3];
                    txtb_salary.Text = data_rows[5];
                    cbb_position.Text = data_rows[6];
                    cbb_department.Text = data_rows[7];
                    txtb_phone.Text = data_rows[8];
                    txtb_id.Text = data_rows[9];
                    txtb_home.Text = data_rows[10];
                    cbb_village.Text = data_rows[11];
                    cbb_district.Text = data_rows[12];
                    cbb_commune.Text = data_rows[13];
                    cbb_province.Text = data_rows[14];
                }   
            }
        }

        private string find_emp_by_code(string code)
        {
            string row = "";
            try
            {
                using (StreamReader read = new StreamReader(db))
                {
                    while (!read.EndOfStream)
                    {
                        row = read.ReadLine();
                        if (row.Contains(code)) break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return row;
        }

        private int get_line_edit_data(string code)
        {
            int row_line = 0; string row = "";
            try
            {
                using (StreamReader read = new StreamReader(db)){
                    while (!read.EndOfStream)
                    {
                        row_line++;
                        row = read.ReadLine();
                        if (row.Contains(code)) break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return row_line;
        }

        private void emp_edit_info(string new_data)
        {
            try
            {
                string emp_code = txtb_student_code.Text;
                int lineNumber = get_line_edit_data(emp_code);
                string[] lines = File.ReadAllLines(db);
                if(lineNumber >= 1 && lineNumber <= lines.Length)
                {
                    lines[lineNumber - 1] = new_data;
                    File.WriteAllLines(db, lines);
                    MessageBox.Show($"Line {lineNumber} successfully edited.");
                }
                else { MessageBox.Show($"Invalid line number: {lineNumber}"); }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured: {e.Message}");
            }
        }

        private void dgv_list_emp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selec_row_data_list(e);
            lbl_save_action.Text = "Edit Mode";
        }
    }
}
