using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
using System.IO;
using System;
using System.Net.Sockets;
using System.Text;

namespace Talker
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Start/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();



            app.UseWebSockets();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Start}/{action=Index}/{id?}");
            });


            if (HybridSupport.IsElectronActive)
            {
                //Task.Run(() => CreateElectronWindowAsync());
                CreateElectronWindowAsync();
            }


        }




        private async void CreateElectronWindowAsync()
        {
            var MainWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {

                Show = false,
                Frame = false,
                Resizable = false,
                Width = 400,
                Height = 565,
                WebPreferences = new WebPreferences
                {
                    AllowRunningInsecureContent = true,
                    WebSecurity = true,
                    NodeIntegration = true
                },

            });
            MainWindow.OnReadyToShow += () => MainWindow.Show();
            MainWindow.SetTitle("Talker");



            var menu = new MenuItem[] {
                new MenuItem
                {
                    Label = "File",
                    Submenu = new MenuItem[]
                    {
                        new MenuItem{
                            Label = "Exit",
                            Click = () => {Electron.App.Exit();}
                        }
                    }
                }
            };

            //Electron.Menu.SetApplicationMenu(menu);

        }

}
}
