using OrderManagementSystemServer.Components.Classes;

namespace OrderManagementSystemServer
{
    public class Program
    {
        public static ServerManager serverManager = new ServerManager();

        public static async Task Main(string[] args)
        {
            await serverManager.Init();            
        }
    }

}