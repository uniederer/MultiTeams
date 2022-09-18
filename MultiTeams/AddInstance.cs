//-----------------------------------------------------------------------
// <copyright company="Niederer Engineering GmbH">
//     Author:  Ueli Niederer
//     Copyright (c) 2022 All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using MultiTeams.Instances;

namespace MultiTeams
{
    public partial class AddInstance : Form
    {
        public InstanceSettings Data { get; } = new InstanceSettings();

        public AddInstance()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Data.Name = tbName.Text;
            Data.Autostart = cbAutostart.Checked;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(tbName.Text);
        }
    }
}