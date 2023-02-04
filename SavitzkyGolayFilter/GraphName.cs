namespace GraphPlotter
{
    public partial class GraphName : Form
    {
        internal string value = "";

        public GraphName()
        {
            InitializeComponent();
        }

        private void GraphName_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
            KeyDown += new KeyEventHandler(GraphName_KeyDown);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            value = textBox1.Text;
        }

        private void GraphName_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
