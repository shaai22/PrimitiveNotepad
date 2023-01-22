using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimitiveNotepad
{
    public partial class UnsavedChanges : Form
    {
        public UnsavedChanges()
        {
            InitializeComponent();
        }

        // Кнопка "сохранить"
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes; // Возвращаем в программу положительный ответ
        }

        // Кнопка "не сохранять"
        private void btnDontSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No; // Возвращаем в программу отрицательный ответ
        }

        // Кнопка "отмена"
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // Возвращаем в программу отмену действия
        }
    }
}
