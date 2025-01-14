using OrderManagementSystemServer.Components.Classes;

namespace OrderManagementSystemServer
{
    public class Program
    {
        public static ServerManager m_objServerManager = new ServerManager();

        public static async Task Main(string[] args)
        {
            await m_objServerManager.Init();            
        }
    }

}