using ToolLib;

namespace FastHex
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void txtHexIn_TextChanged(object sender, EventArgs e)
        {
            txtHexOut.Clear();
            foreach (var line in txtHexIn.Lines)
            {
                try
                {
                    txtHexOut.Text += Convert.ToUInt64(line.Split(":")[0].Split(",")[0], 16) + Environment.NewLine;
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void txtNumIn_TextChanged(object sender, EventArgs e)
        {
            txtNumOut.Clear();
            foreach (var line in txtNumIn.Lines)
            {
                try
                {
                    var val = Convert.ToUInt64(line.Split(":")[0].Split(",")[0]).ToString("X8");
                    if (chkHashcat.Checked)
                    {
                        val += ":00000000";
                    }
                    txtNumOut.Text += val + Environment.NewLine;
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void txtMmhIn_TextChanged(object sender, EventArgs e)
        {
            txtMmhOut.Clear();
            foreach (var line in txtMmhIn.Lines)
            {
                try
                {
                    txtMmhOut.Text += MurMurHash3.Hash(line) + Environment.NewLine;
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
