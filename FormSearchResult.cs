using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veterinarska_stanica__.NET_
{
    public partial class FormSearchResult : Form
    {
        public FormSearchResult()
        {
            InitializeComponent();
        }

        public FormSearchResult(DataTable dt) : this()
        {
            dataGridView2.DataSource = dt;
        }
    }
}
