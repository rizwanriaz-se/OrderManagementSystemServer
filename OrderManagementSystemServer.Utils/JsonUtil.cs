using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderManagementSystemServer.Utils
{
    public class JsonUtil
    {
        //private string? SerializeJson(Response response)
        //{
        //    try
        //    {
        //        if (response != null)
        //            return JsonSerializer.Serialize(response);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Unexpected error occurred when trying to serialize data.");
        //        return null;
        //    }
        //}

        //private T? DeserializeJson<T>(object responseData)
        //{
        //    try
        //    {
        //        if (responseData != null)
        //        {
        //            string data = responseData.ToString();
        //            return (T)JsonSerializer.Deserialize<T>(data);
        //        }
        //        return default;
        //    }
        //    catch (Exception)
        //    {
        //        Debug.WriteLine("Unexpected error occurred when trying to deserialize data.");
        //        return default;
        //    }
        //}
    }
}
