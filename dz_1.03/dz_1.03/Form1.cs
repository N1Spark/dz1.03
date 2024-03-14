using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using EagerLoadinf_Library_ADO_Winforms.Model;

namespace dz_1._03
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ShowAll();
        }

        public void ShowAll()
        {
            textBox6.Text = "";
            using (LibraryEagerEntities bc = new LibraryEagerEntities())
            {
                foreach (var info in bc.Books.Include("Autors").Include("Categories").Include("Publishings"))
                {
                    string authors = string.Join(", ", info.Autors.Name);
                    string categories = string.Join(", ", info.Categories.Name);
                    string publishings = string.Join(", ", info.Publishings.Name);
                    textBox6.AppendText($"Name:{info.Name}\r\n" +
                        $"Autor:{authors}\r\n" +
                        $"Page:{info.PageCount}\r\n" +
                        $"Categories:{categories}\r\n" +
                        $"Publishings:{publishings}\r\n\r\n");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                Categories categories = db.Categories.FirstOrDefault(c => c.Name == textBox2.Text);
                if (categories == null)
                {
                    categories = new Categories { Name = textBox2.Text };
                    db.Categories.Add(categories);
                }
                else
                    db.Entry(categories).Collection(c => c.Books).Load();
                Publishings publisher = db.Publishings.FirstOrDefault(p => p.Name == textBox3.Text);
                if (publisher == null)
                {
                    publisher = new Publishings { Name = textBox3.Text };
                    db.Publishings.Add(publisher);
                }
                else
                    db.Entry(publisher).Collection(p => p.Books).Load();
                Autors author = db.Autors.FirstOrDefault(a => a.Name == textBox5.Text);
                if (author == null)
                {
                    author = new Autors { Name = textBox5.Text };
                    db.Autors.Add(author);
                }
                else
                    db.Entry(author).Collection(a => a.Books).Load();
                Books book = new Books
                {
                    Name = textBox1.Text,
                    Categories = categories,
                    Publishings = publisher,
                    PageCount = Convert.ToInt32(textBox4.Text),
                    Autors = author
                };
                db.Books.Add(book);
                db.SaveChanges();
                ShowAll();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                Books books = db.Books.FirstOrDefault(n => n.Name == textBox7.Text);
                if (books != null)
                {
                    db.Books.Remove(books);
                    db.SaveChanges();
                }
                else
                    MessageBox.Show("Не удалось найти книгу");
                ShowAll();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                Autors autor = db.Autors.FirstOrDefault(n => n.Name.Contains(textBox8.Text));
                if (autor != null)
                {
                    db.Entry(autor).Collection(a => a.Books).Load();
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var book in autor.Books)
                        stringBuilder.AppendLine($"Name:{book.Name},Page:{book.PageCount}\n");
                    textBox6.Text = stringBuilder.ToString();
                }
                else
                    textBox6.Text = "Нет результатов";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                Books book = db.Books.FirstOrDefault(b => b.Name.Contains(textBox8.Text));
                if (book != null)
                {
                    db.Entry(book).Reference(b => b.Categories).Load();
                    db.Entry(book).Reference(b => b.Publishings).Load();
                    db.Entry(book).Reference(b => b.Autors).Load();
                    textBox6.Text = $"Название: {book.Name}\r\n" +
                                       $"Категория: {book.Categories.Name}\r\n" +
                                       $"Издательство: {book.PublisherId}\r\n" +
                                       $"Количество страниц: {book.PageCount}\r\n" +
                                       $"Автор: {book.Autors.Name}\r\n";
                }
                else
                    textBox6.Text = "Нет результатов";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                var books = db.Books.Where(b => b.PageCount == Convert.ToInt32(textBox4.Text)).ToList();
                if (books.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var book in books)
                        sb.AppendLine($"Название: {book.Name}\r, Кол-во страниц:{book.PageCount}");
                    textBox6.Text = sb.ToString();
                }
                else
                    textBox6.Text = "Нет результатов";
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            using (LibraryEagerEntities db = new LibraryEagerEntities())
            {
                Publishings publisher = db.Publishings.FirstOrDefault(p => p.Name.Contains(textBox8.Text));
                if (publisher != null)
                {
                    db.Entry(publisher).Collection(p => p.Books).Load();
                    StringBuilder sb = new StringBuilder();
                    foreach (var book in publisher.Books)
                        sb.AppendLine($"Name:{book.Name}");
                    textBox6.Text = sb.ToString();
                }
                else
                    textBox6.Text = "Нет результатов";
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            ShowAll();
        }
    }
}
