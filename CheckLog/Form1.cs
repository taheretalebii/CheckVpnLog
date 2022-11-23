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

namespace CheckLog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnCheckFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog df = new OpenFileDialog();
            List<LogDto> lst = new List<LogDto>();
            if (df.ShowDialog() == DialogResult.OK)
            {
                string logtext = File.ReadAllText(df.FileName);
                logtext.Split('\n').ToList().ForEach(line =>
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        var username = line.Substring(line.LastIndexOf("GET /") + 5, line.IndexOf(" HTTP") - line.IndexOf("GET /") - 5);
                        var datestr = line.Substring(line.IndexOf("[") + 1,
                         line.IndexOf("]") - line.IndexOf("[") - 1);
                        var date = DateTime.ParseExact(datestr, "dd/MMM/yyyy:HH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture);
                        var ip = line.Substring(0, line.IndexOf(" - - "));

                        var exist = lst.FirstOrDefault(p => p.Date.Year == date.Year && p.Date.Month == date.Month &&
                        p.Date.Day == date.Day && p.Date.Hour == date.Hour
                        && p.Date.Minute == date.Minute
                        && p.Date.Second == date.Second
                        && !p.IP.Split(',').Contains(ip));
                        if (exist != null)
                        {
                            exist.IsDuplicate = true;
                            exist.IP += $",{ip}";
                        }
                        else
                            lst.Add(new LogDto() { Date = date, IP = ip, IsDuplicate = false ,Username = username });


                    }

                });
                if (lst.Any(p => p.IsDuplicate))
                {
                    string fname = $"log_{DateTime.Now.Ticks}.txt";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), fname);
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(string.Join("\n", lst.Where(p => p.IsDuplicate).Select(p =>"Username: " + p.Username + " Date: " + p.Date + " ====> IP List : " + p.IP)));
                    }
                    MessageBox.Show("Task is completed!");
                }


            }
        }
    }


}
