namespace ConsoleApp1.Saver.Interface;

public interface IDataSaver
{
    T Load<T>(string filePath) where T : class;
    void Save<T>(string filePath, T item) where T : class;
}