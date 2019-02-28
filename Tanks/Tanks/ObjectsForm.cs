using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tanks
{
    public partial class ObjectsForm : Form
    {
        MainWindow parent;
        int delay;

        public ObjectsForm(MainWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
            parent.OnUpdate += _Update;
        }


        void _Update()
        {
            if (delay <= 0)
            {
                dataGridView1.DataSource = parent.game.Objects.ToList();
                delay = 15;
            }
            else
            {
                delay--;
            }
        }
    }
}
