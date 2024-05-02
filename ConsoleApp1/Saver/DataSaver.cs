using ConsoleApp1.Saver.Interface;
using Newtonsoft.Json;

namespace ConsoleApp1.Saver;

public class DataSaver : IDataSaver
{
    public T Load<T>(string filePath) where T : class
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "Путь до файла не может быть пустым!");
        }
        
        using var sr = new StreamReader(filePath);
        return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
    }

    public void Save<T>(string filePath, T item) where T : class
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentNullException(nameof(filePath), "Путь до файла не может быть пустым!");
        }

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Данные для сохранения не могут быть NULL!");
        }

        var text = JsonConvert.SerializeObject(item);
        File.WriteAllText(filePath, text);
    }
}