using SecurityProject.Storage;

namespace SecurityProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileManager.InitializeFiles();
            Menu menu = new Menu();
            Console.WriteLine(menu);
        }
    }
}
