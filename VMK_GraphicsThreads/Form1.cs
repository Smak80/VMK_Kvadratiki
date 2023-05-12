namespace VMK_GraphicsThreads
{
    public partial class Form1 : Form
    {
        private Animator animator;
        public Form1()
        {
            InitializeComponent();
            animator = new Animator(panel1.CreateGraphics());
            animator.Start();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            animator.AddNewBox();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            animator.MainGraphics = panel1.CreateGraphics();
        }
    }
}