﻿using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBot
{
    public static class ImageAnalyzer
    {
        public static async Task<AnalysisResult> DescribeImage(string url, string serviceUrl)
        {
            try
            {
                var downloader = new HttpClient();
                
                var uri = new Uri(url);
               // if (uri.Host.EndsWith("skype.com") && uri.Scheme == "https")
                {
                    
                    using (var connectorClient = new ConnectorClient(new Uri(serviceUrl)))
                    {
                        var token = await (connectorClient.Credentials as MicrosoftAppCredentials).GetTokenAsync();



                        if (!string.IsNullOrEmpty(token))
                        {
                            downloader.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                        }
                    }
                }
                var imageStream = await downloader.GetStreamAsync(url);
                var subscriptionKey = ConfigurationManager.AppSettings["MsCognitive:ComputerVision"];
                var client = new VisionServiceClient(subscriptionKey);
                var features = new VisualFeature[] { VisualFeature.Color, VisualFeature.Description };
                var result = await client.AnalyzeImageAsync(imageStream, features);

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            

        }
    }
}
