using ElectronNET.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talker.Models;
using Talker.Models.DTO;

namespace Talker.Controllers.Other
{
    public static class HelperMethods
    {

        public static void DisplayErrorMessage(Exception exception, BrowserWindow browserWindow, string channel, bool displayDislogError)
        {

            if(exception is ExceptionRestStatus)
            {

                ExceptionRestStatus ex = (ExceptionRestStatus)exception;

                Console.WriteLine("WARNING: API returned error status");

                string ret = ex.Message;
                var inner = ex.InnerException;

                while (inner != null)
                {
                    ret += (inner.Message + "\r\n");
                    inner = inner.InnerException;
                }

                Console.WriteLine(JsonConvert.SerializeObject(ex.toRestStatus()).ToString());

                Electron.IpcMain.Send(browserWindow, channel, (JsonConvert.SerializeObject(ex.toRestStatus()).ToString()));

                if(displayDislogError) Electron.Dialog.ShowErrorBox(String.Format("{0} ({1})\t", ex.error, ex.status), ret);

            }
            else
            {

                Console.WriteLine("WARNING: Exception occured");

                string ret = "";
                Console.WriteLine("->" + exception.Message);

                var inner = exception.InnerException;

                while (inner != null)
                {
                    Console.WriteLine("-->" + inner.Message);

                    ret += (inner.Message + "\r\n");
                    inner = inner.InnerException;
                }

                Electron.IpcMain.Send(browserWindow, channel, JsonConvert.SerializeObject(new RestStatus { errortype = "application", error = exception.Message, message = ret }).ToString());

                if (displayDislogError) Electron.Dialog.ShowErrorBox(exception.Message, ret);

            }
   

        }



        public static async Task<BrowserWindow> GetFinwodWindowAsync(string windowPath)
        {
            string currentWindowUrl = ($"http://localhost:{BridgeSettings.WebPort}/{windowPath}");

            BrowserWindow[] browserWindows = Electron.WindowManager.BrowserWindows.ToArray();

            for (int i = 0; i < browserWindows.Length; i++)
            {

                BrowserWindow bw = browserWindows[i];
                string url = await bw.WebContents.GetUrl();

                Console.WriteLine($">>>>>Current URL:{url} <<<<<<>>>>> looking for url: {currentWindowUrl}");

                if (url.Contains(currentWindowUrl))
                {
                    return bw;
                }

            }

            return null;
        }


    }
}
