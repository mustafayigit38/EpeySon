﻿using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public static class CompareItems
    {
        static CompareItems()
        {
            CompareItemId = new List<string>();
        }

        public static List<string> CompareItemId { get; set; }
    }
}
