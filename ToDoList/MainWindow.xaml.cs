using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoList.Models;
using ToDoList.Services;

namespace ToDoList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\todoDataList.json"; // Файл всегда будет лежать рядом с .exe, поэтому мы обращаемся к классу Environment (окружение)
                                                                                             // к Сurrent directory (текущему каталогу) и будем добавлять todoDataList.json
        private BindingList<TodoModel> _todoDataList;
        private FileIOService _fileIOService;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window_Loaded(sender, e, _todoDataList);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e, BindingList<TodoModel> _todoDataList)
        {

            _fileIOService = new FileIOService(PATH); // в конструктор этого класса необходимо передать путь, по которому должен лежать файл, то есть PATH


            // Если мы вдруг получаем исключение, то эта информация выводится пользлвателю, и приложение закрывается
            try
            {
                _todoDataList = _fileIOService.LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }


            _todoDataList = _fileIOService.LoadData();


            dgTodoList.ItemsSource = _todoDataList;
            _todoDataList.ListChanged += _todoDataList_ListChanged;


        }

        private void _todoDataList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted || e.ListChangedType == ListChangedType.ItemChanged)
            {
                try
                {
                    _fileIOService.SaveData(sender);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Close();
                }
            }

        }
    }
}
