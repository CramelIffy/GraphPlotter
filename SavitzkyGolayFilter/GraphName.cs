namespace GraphPlotter
{
    public partial class GraphName : Form
    {
        internal string value;

        public GraphName()
        {
            InitializeComponent();
        }

        private void GraphName_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = textBox1.Text;
            if (value != "")
                Close();
            else MessageBox.Show("グラフの名前を入力してください。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
