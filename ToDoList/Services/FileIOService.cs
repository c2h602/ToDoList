using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;




/* В этом классе будет реализовано два метода: 
 * 1 Метод, который будет считывать данные с жд
 * 2 Метод, сохраняющий данные на жд */


namespace ToDoList.Services
{
    internal class FileIOService
    {

        private readonly string PATH;

        public FileIOService(string path) // Передаем путь, по которому будет сохраняться файл
        {
            PATH = path;
        }


        public BindingList<TodoModel> LoadData() // Загрузка файла: 
                                                  
        {
            bool fileExists = File.Exists(PATH);  

            if (!fileExists)                     // 1 Проверка на существование перед чтением файла
            {                                    // 2 Результат проверки на сущ будет хранится в булевой переменной fileExist
                File.CreateText(PATH).Dispose(); // 3 Если файла не сущ, то считать файл мы не можем, тогда создаем этот файл и биндинг лист с пустым списком и возвращаем его
                return new BindingList<TodoModel>();
            }

            


            using (var reader = File.OpenText(PATH)) // 4 Если файл сущ, то выполняем чтение с помощью .OpenText,
            {
                var fileText = reader.ReadToEnd(); // данные считываем в строку,
                return JsonConvert.DeserializeObject<BindingList<TodoModel>>(fileText); // и десериализируем с помощью класса JsonConvert в коллекцию BindingList,
                                                                                        // которая содержит модель записи  
            }
        }

        public void SaveData(object todoDataList) // Метод, сохраняющий данные в файл
        {
            using (StreamWriter writer = File.CreateText(PATH)) /* Используем объект writer класса StreamWriter, который создаем путем вызова метода CreateText у класс File,
                                                                 * при этом передаем переменную PATH, в которой хранится путь к файлу.
                                                                 * using используется для того, чтобы вызвать метод Dispose объекта writer, который освобождает ресурсы для записи на диск */
            {
                string output = JsonConvert.SerializeObject(todoDataList); // Список дел десериализуется с помощью джсон конвертера в строку
                writer.Write(output); // А здесь эта строка записывается в файл
            }
        }

    
    }
}
