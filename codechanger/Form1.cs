using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace codechanger
{
    public partial class Form1 : Form
    {
        public class CodeEntry
        {
            public string Name { get; set; }
            public string S { get; set; }
            public string R { get; set; }
        }

        private List<CodeEntry> codeEntries = new List<CodeEntry>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private string ConvertHexSeqToAob(string hexSeq)
        {
            if (string.IsNullOrWhiteSpace(hexSeq))
                return string.Empty;

            string cleanedSeq = hexSeq.Replace("\\x", "").Trim();

            var aobSegments = new List<string>();

            for (int i = 0; i < cleanedSeq.Length; i += 2)
            {
                if (i + 1 >= cleanedSeq.Length)
                {
                    throw new ArgumentException("Invalid hexadecimal sequence (missing characters at the end).");
                }

                string segment = cleanedSeq.Substring(i, 2);

                if (segment == ".." || segment.Contains("."))
                {
                    aobSegments.Add("??");
                }
                else
                {
                    aobSegments.Add(segment.ToUpper());
                }
            }

            return string.Join(" ", aobSegments);
        }

        private string ConvertAobToHexSeq(string aob)
        {
            if (string.IsNullOrWhiteSpace(aob))
                return string.Empty;

            var segments = aob.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (s.Trim() == "??" || s.Trim() == "?")
                        return ".";

                    if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out _))
                        return $"\\x{s.PadLeft(2, '0').ToUpper()}";

                    throw new ArgumentException($"Invalid AOB segment: {s}");
                });

            return string.Join("", segments);
        }

        private string ConvertCppBytesToHexSeq(string cppBytes)
        {
            if (string.IsNullOrWhiteSpace(cppBytes))
                return string.Empty;

            string cleanedArray = cppBytes
                .Replace("0x", "")
                .Replace(",", "")
                .Trim();

            var segments = cleanedArray
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => $"\\x{s.ToUpper()}");

            return string.Join("", segments);
        }

        private string ConvertHexSeqToCppBytes(string hexSeq)
        {
            if (string.IsNullOrWhiteSpace(hexSeq))
                return string.Empty;

            string cleanedSeq = hexSeq.Replace("\\x", "").Trim();

            var bytes = new List<string>();

            for (int i = 0; i < cleanedSeq.Length; i += 2)
            {
                if (i + 1 >= cleanedSeq.Length)
                {
                    throw new ArgumentException("Invalid hexadecimal sequence (missing characters at the end).");
                }

                string segment = cleanedSeq.Substring(i, 2);

                if (segment == "?" || segment.Contains("?"))
                {
                    bytes.Add("'?'");
                }
                else
                {
                    bytes.Add($"0x{segment.ToLower()}");
                }
            }

            return string.Join(", ", bytes);
        }

        private string ConvertAobToBytes(string aob)
        {
            if (string.IsNullOrWhiteSpace(aob))
                return string.Empty;

            var segments = aob.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (s.Trim() == "?" || s.Trim() == "??")
                        return "'?'";

                    if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out _))
                        return $"0x{s.PadLeft(2, '0').ToUpper()}";

                    throw new ArgumentException($"Invalid AOB segment: {s}");
                });

            return string.Join(", ", segments);
        }

        private string ConvertBytesToAob(string byteArray)
        {
            if (string.IsNullOrWhiteSpace(byteArray))
                return string.Empty;

            string cleanedArray = byteArray
                .Replace("std::vector<BYTE> bytes = {", "")
                .Replace("};", "")
                .Replace("{", "")
                .Replace("}", "")
                .Trim();

            var segments = cleanedArray
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    if (s.Trim() == "'?'" || s.Trim() == "?")
                        return "?";

                    s = s.Replace("0x", "")
                         .Replace("0X", "")
                         .Trim();

                    if (int.TryParse(s, System.Globalization.NumberStyles.HexNumber, null, out _))
                        return s.PadLeft(2, '0').ToUpper();

                    throw new ArgumentException($"Invalid byte segment: {s}");
                });

            return string.Join(" ", segments);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string byteArray = ConvertAobToBytes(inputCode);
                guna2TextBox6.Text = byteArray;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string aob = ConvertBytesToAob(inputCode);
                guna2TextBox6.Text = aob;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string hexSeq = ConvertAobToHexSeq(inputCode);
                guna2TextBox6.Text = hexSeq;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string hexSeq = ConvertCppBytesToHexSeq(inputCode);
                guna2TextBox6.Text = hexSeq;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string aob = ConvertHexSeqToAob(inputCode);
                guna2TextBox6.Text = aob;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string inputCode = guna2TextBox8.Text.Trim();
            try
            {
                string bytes = ConvertHexSeqToCppBytes(inputCode);
                guna2TextBox6.Text = bytes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while converting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
