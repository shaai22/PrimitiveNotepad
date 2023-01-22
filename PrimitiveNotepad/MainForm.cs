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
        // Немного из курса по ООП:

        // Создаём поля:
        public bool documentSaved = true; // Документ сохранён? (по умолчанию: "правда")

        public string currentDocument = ""; // Имя текущего файла (по умолчанию: пустая строка - нет открытого файла)

        // Подключаем нужные нам формы:
        public UnsavedChanges dlgUnsaved = new UnsavedChanges(); // Форма "несохраненный файл" (собственная форма)

        public OpenFileDialog dlgOpen = new OpenFileDialog(); // Диалог открытия файла

        public SaveFileDialog dlgSave = new SaveFileDialog(); // Диалог сохранения файла

        public FontDialog dlgFont = new FontDialog(); // Диалог выбора шрифта

        public MainForm()
        {
            InitializeComponent();

            // Настраиваем "фильтр" для открытия/сохранения, чтобы он показывал только текст или все файлы
            dlgOpen.Filter = "Текстовые документы|*.txt|Все файлы|*.*";
            dlgSave.Filter = "Текстовые документы|*.txt|Все файлы|*.*";
        }

        private void mainText_TextChanged(object sender, EventArgs e)
        {
            documentSaved = false; // Когда в поле ввода текста меняется содержимое, меняем статус сохранения на "ложь"
        }

        // Выбран пункт меню "Файл" - "Новый"
        private void menuNew_Click(object sender, EventArgs e)
        {
            if (!documentSaved) // Если документ не сохранён, тогда:
            {
                dlgUnsaved.ShowDialog(); // Спрашиваем пользователя, хочет ли он сохранить файл

                switch (dlgUnsaved.DialogResult) // Действуем в соответствии с ответом на диалог:
                {
                    case DialogResult.Yes: // Кнопка "сохранить"

                        saveFile(); // Запускаем процедуру сохранения файла
                        break;
                    case DialogResult.No: // Кнопка "не сохранять"

                        resetText(); // Сбрасываем состояние программы
                        break;
                    case DialogResult.Cancel: // Кнопка "отмена" - ничего не делаем
                        break;
                }
            } else // ...а если сохранен, тогда:
            {
                resetText(); // Сбрасываем состояние программы
            }
        }

        // Выбран пункт меню "Файл" - "Открыть"
        private void menuOpen_Click(object sender, EventArgs e)
        {
            if (!documentSaved) // Если документ не сохранён, тогда:
            {
                dlgUnsaved.ShowDialog(); // Спрашиваем пользователя, хочет ли он сохранить файл

                switch (dlgUnsaved.DialogResult) // Действуем в соответствии с ответом на диалог:
                {
                    case DialogResult.Yes: // Кнопка "сохранить"

                        saveFile(); // Запускаем процедуру сохранения файла
                        break;
                    case DialogResult.No: // Кнопка "не сохранять"

                        openFile(); // Запускаем процедуру открытия файла
                        break;
                    case DialogResult.Cancel: // Кнопка "отмена" - ничего не делаем
                        break;
                }
            } else // ...а если сохранен, тогда:
            {
                openFile(); // Запускаем процедуру открытия файла
            }
        }

        // Выбран пункт меню "Файл" - "Сохранить"
        private void menuSave_Click(object sender, EventArgs e)
        {
            if (!documentSaved) // Если документ не сохранён, тогда:
            {
                if (currentDocument != "") // Если открыт какой-либо файл, тогда:
                {
                    // Просто записываем содержимое текстового поля в текущий файл
                    using (StreamWriter sw = new StreamWriter(currentDocument))
                    {
                        sw.WriteLine(mainText.Text);
                    }
                } else
                {
                    saveFile(); // Запускаем процедуру сохранения файла
                }
            } else // ...а если сохранён, тогда ничего не делаем
            {
                return;
            }
        }

        // Выбран пункт меню "Файл" - "Сохранить как"
        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            saveFile(); // Запускаем процедуру сохранения файла
        }

        // Выбран пункт меню "Файл" - "Выход"
        private void menuExit_Click(object sender, EventArgs e)
        {
            if (!documentSaved) // Если документ не сохранён, тогда:
            {
                dlgUnsaved.ShowDialog(); // Спрашиваем пользователя, хочет ли он сохранить файл

                switch (dlgUnsaved.DialogResult) // Действуем в соответствии с ответом на диалог:
                {
                    case DialogResult.Yes: // Кнопка "сохранить"

                        saveFile(); // Запускаем процедуру сохранения файла

                        this.Close(); // Выходим из программы
                        break;
                    case DialogResult.No: // Кнопка "не сохранять" - просто выходим из программы
                        this.Close();
                        break;
                    case DialogResult.Cancel: // Кнопка "отмена" - ничего не делаем
                        break;
                }
            }
            else // ...а если сохранён, тогда просто выходим из программы
            {
                this.Close();
            }
        }

        // Выбран пункт меню "Вид" - "Шрифт"
        private void menuFont_Click(object sender, EventArgs e)
        {
            dlgFont.ShowDialog(); // Показываем окно выбора шрифта

            mainText.Font = dlgFont.Font; // Устанавливаем шрифт текстового поля в выбранный пользователем параметр
        }

        // Выбран пункт меню "Вид" - "Перенос по словам"
        private void menuWrap_Click(object sender, EventArgs e)
        {
            mainText.WordWrap = !mainText.WordWrap; // Включаем (или выключаем) перенос по словам в текстовом поле

            menuWrap.Checked = !menuWrap.Checked; // Ставим (снимаем) галочку в пункте меню
        }

        // Процедура сброса состояния программы
        public void resetText()
        {
            mainText.Text = ""; // Обнуляем текстовое поле

            currentDocument = ""; // Сбрасываем имя открытого файла

            documentSaved = true; // Устанавливаем поле "документ сохранен"
        }

        // Процедура сохранения файла
        public void saveFile()
        {
            dlgSave.ShowDialog(); // Показываем диалог сохранения файла

            if (dlgSave.FileName != "") // Если введено имя файла
            {
                // Записываем данные из текстового поля в файл на диске
                using (StreamWriter sw = new StreamWriter(dlgSave.FileName))
                {
                    sw.WriteLine(mainText.Text);
                }

                documentSaved = true; // Устанавливаем поле "документ сохранен"
            }
        }

        // Процедура открытия файла
        public void openFile()
        {
            dlgOpen.ShowDialog(); // Показываем диалог открытия файла

            if (dlgOpen.FileName != "") // Если введено имя файла
            {
                currentDocument = dlgOpen.FileName; // Записываем имя файла в поле открытого документа

                // Читаем данные из файла и заполняем текстовое поле
                using (StreamReader sr = new StreamReader(dlgOpen.FileName))
                {
                    mainText.Text = sr.ReadToEnd();
                }

                documentSaved = true; // Устанавливаем поле "документ сохранен"
            }
        }
    }
}
