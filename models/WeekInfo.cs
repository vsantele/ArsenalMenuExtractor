using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using HtmlAgilityPack;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;

namespace Vsantele.Functions.Models
{
    public class WeekInfo
    {
        public string WeekNumber { get; set; }
        public string DayStart { get; set; }
        public string MonthStart { get; set; }
        public string DayEnd { get; set; }
        public string MonthEnd { get; set; }
        public string Year { get; set; }
    }
}