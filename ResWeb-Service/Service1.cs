using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using System;
using System.IO;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace ResWeb_BackendService
{

    public class data
    {
        public Int32 time { get; set; }
        public string PC_Name { get; set; }
        public string Mainboard_Name { get; set; }
        public string CPU_Name { get; set; }
        public string RAM_Name { get; set; }
        public string GPU_Name { get; set; }
        public string CPU_Temp { get; set; }
        public string GPU_Temp { get; set; }
        public string CPU_Usage { get; set; }
        public string RAM_Usage { get; set; }
        public string CPU_Clock { get; set; }
        public string RAM_Used { get; set; }
        public string RAM_Available { get; set; }
    }

    public partial class Service1 : ServiceBase
    {
        #region strings
        static string urlregister = "http://134.3.87.59:8899/api/data/register";
        static string urlcollect = "http://134.3.87.59:8899/api/data/collect";


        static bool onstart = true;

        static int id = 0;


        #endregion

        #region handlers


        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath =
                AppDomain.CurrentDomain.BaseDirectory
                + "\\Logs\\ServiceLog_"
                + DateTime.Now.Date.ToShortDateString().Replace('/', '_')
                + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }




        #endregion

        #region service

        protected override void OnStart(string[] args)
        {
            

            WriteToFile("Service is started at " + DateTime.Now);
            Computer computer = new Computer()
            {
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true,
                FanControllerEnabled = true,
                MainboardEnabled = true
            };
            computer.Open();

            System.Timers.Timer timer = new System.Timers.Timer()
            {
                Enabled = true,
                Interval = 1000
            };
            timer.Elapsed += delegate(object sender, ElapsedEventArgs e)
            {
                var SystemData = new data();

                SystemData.PC_Name = Environment.MachineName;
                SystemData.time = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                foreach (IHardware hardware in computer.Hardware)
                {
                    hardware.Update();

                    if (hardware.HardwareType.ToString() == "Mainboard")
                    {
                        SystemData.Mainboard_Name = hardware.Name;
                    }

                    if (hardware.HardwareType.ToString() == "CPU")
                    {
                        SystemData.CPU_Name = hardware.Name;
                    }
                    if (hardware.HardwareType.ToString() == "RAM")
                    {
                        SystemData.RAM_Name = hardware.Name;
                    }
                    if (
                        hardware.HardwareType
                            .ToString()
                            .IndexOf("gpu", 0, StringComparison.OrdinalIgnoreCase) != -1
                    )
                    {
                        SystemData.GPU_Name = hardware.Name;
                    }

                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            if (sensor.Name == "CPU Core #1") // was wir wollen //cpu core clock
                            {
                                SystemData.CPU_Clock = $"{sensor.Value?.ToString("0.00")} MHz";
                            }
                            else //der rest
                            {
                                continue;
                            }
                        }
                        if (sensor.SensorType == SensorType.Load)
                        {
                            if (sensor.Name == "CPU Total") // was wir wollen //cpu load
                            {
                                SystemData.CPU_Usage = $"{sensor.Value?.ToString("0.00")} %";
                            }
                            else if (sensor.Name == "Memory")
                            {
                                SystemData.RAM_Usage = $"{sensor.Value?.ToString("0.00")} %";
                            }
                            else //der rest
                            {
                                continue;
                            }

                            

                        }
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (sensor.Name == "CPU Package") // was wir wollen //cpu temp
                            {
                                SystemData.CPU_Temp = $"{sensor.Value?.ToString("0.00")} °C";
                            }
                            if (sensor.Name == "GPU Core")
                            {
                                SystemData.GPU_Temp = $"{sensor.Value?.ToString("0.00")} °C";
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (sensor.SensorType == SensorType.Fan) { }

                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Name == "Used Memory")
                            {
                                SystemData.RAM_Used = $"{sensor.Value?.ToString("0.00")} GB";
                            }
                            if (sensor.Name == "Available Memory")
                            {
                                SystemData.RAM_Available = $"{sensor.Value?.ToString("0.00")} GB";
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                string registerJson = JsonConvert.SerializeObject(new { SystemData });



                void getId()
                {
                    if (id != 0)
                    {
                        return; // ID already fetched
                    }

                    // Fetch the ID from the server
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                        try
                        {
                            var response = httpClient
                                .PostAsync(urlregister, new StringContent(registerJson, Encoding.UTF8, "application/json"))
                                .Result;


                            if (response.IsSuccessStatusCode)
                            {
                                id = int.Parse(response.Content.ReadAsStringAsync().Result);
                                WriteToFile("Got ID: " + id);
                            }
                            else
                            {
                                WriteToFile("Error: " + response.StatusCode.ToString());
                                Environment.Exit(0);
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteToFile("Error: " + ex.Message);
                        }
                    }
                }
                getId();

                


                string collectJson = JsonConvert.SerializeObject(new { id, SystemData });
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    try
                    {
                        WriteToFile(collectJson);
                        var response = httpClient
                            .PostAsync(urlcollect, new StringContent(collectJson, Encoding.UTF8, "application/json"))
                            .Result;



                       
                    }
                    catch (Exception ex)
                    {
                        WriteToFile("Error: " + ex.Message);
                    }
                }



            };
        }

        protected override void OnStop() { }
        #endregion
    }
}
