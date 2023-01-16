using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrimitiveNotepad
{
    public partial class MainForm : Form
    {

        public bool documentSaved = true; // Документ сохранён?

        public string currentDocument = ""; // Текущий документ

        public UnsavedChanges dlgUnsaved = new UnsavedChanges();
        public OpenFileDialog dlgOpen = new OpenFileDialog();
        public SaveFileDialog dlgSave = new SaveFileDialog();
        public FontDialog dlgFont = new FontDialog();
        public MainForm()
        {
            InitializeComponent();
            dlgOpen.Filter = "Текстовые документы|*.txt|Все файлы|*.*";
            dlgSave.Filter = "Текстовые документы|*.txt|Все файлы|*.*";
        }

        private void mainText_TextChanged(object sender, EventArgs e)
        {
            documentSaved = false;
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            if (!documentSaved)
            {
                dlgUnsaved.ShowDialog();
                switch (dlgUnsaved.DialogResult)
                {
                    case DialogResult.Yes:
                        dlgSave.ShowDialog();
                        if (dlgSave.FileName != "")
                        {
                            using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                            {
                                sw.WriteLine(mainText.Text);
                            }
                            documentSaved = true;
                        }
                        break;
                    case DialogResult.No:
                        mainText.Text = "";
                        currentDocument = "";
                        documentSaved = true;
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            } else
            {
                mainText.Text = "";
                currentDocument = "";
                documentSaved = true;
            }
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            if (!documentSaved)
            {
                dlgUnsaved.ShowDialog();
                switch (dlgUnsaved.DialogResult)
                {
                    case DialogResult.Yes:
                        dlgSave.ShowDialog();
                        if (dlgSave.FileName != "")
                        {
                            using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                            {
                                sw.WriteLine(mainText.Text);
                            }
                            documentSaved = true;
                        }
                        break;
                    case DialogResult.No:
                        dlgOpen.ShowDialog();
                        if (dlgOpen.FileName != "")
                        {
                            currentDocument = dlgOpen.FileName;
                            using (StreamReader sr = new StreamReader(dlgOpen.FileName))
                            {
                                mainText.Text = sr.ReadToEnd();
                            }
                            documentSaved = true;
                        }
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            } else
            {
                dlgOpen.ShowDialog();
                if (dlgOpen.FileName != "")
                {
                    currentDocument = dlgOpen.FileName;
                    using (StreamReader sr = new StreamReader(dlgOpen.FileName))
                    {
                        mainText.Text = sr.ReadToEnd();
                    }
                    documentSaved = true;
                }
            }
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            if (!documentSaved)
            {
                if (currentDocument != "")
                {
                    using (StreamWriter sw = new StreamWriter(currentDocument))
                    {
                        sw.WriteLine(mainText.Text);
                    }
                } else
                {
                    dlgSave.ShowDialog();
                    if (dlgSave.FileName != "")
                    {
                        using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                        {
                            sw.WriteLine(mainText.Text);
                        }
                        documentSaved = true;
                    }
                }
            } else
            {
                return;
            }
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            dlgSave.ShowDialog();
            if (dlgSave.FileName != "")
            {
                using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                {
                    sw.WriteLine(mainText.Text);
                }
                documentSaved = true;
            }
            currentDocument = dlgSave.FileName;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            if (!documentSaved)
            {
                dlgUnsaved.ShowDialog();
                switch (dlgUnsaved.DialogResult)
                {
                    case DialogResult.Yes:
                        dlgSave.ShowDialog();
                        if (dlgSave.FileName != "")
                        {
                            using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                            {
                                sw.WriteLine(mainText.Text);
                            }
                            this.Close();
                        }
                        break;
                    case DialogResult.No:
                        this.Close();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else
            {
                this.Close();
            }
        }

        private void menuFont_Click(object sender, EventArgs e)
        {
            dlgFont.ShowDialog();
            mainText.Font = dlgFont.Font;
        }

        private void menuWrap_Click(object sender, EventArgs e)
        {
            if (menuWrap.Checked)
            {
                mainText.WordWrap = false;
                menuWrap.Checked = false;
            } else
            {
                mainText.WordWrap = true;
                menuWrap.Checked = true;
            }
        }
    }
}
