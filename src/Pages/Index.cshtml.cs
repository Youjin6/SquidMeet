﻿using System.Collections.Generic;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// Yucong Li
    /// Selina Hu
    /// JenChieh Lu
    /// Laharika Kalyani
    /// </summary>

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger,
            JsonFileLocationService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        public JsonFileLocationService ProductService { get; }
        public IEnumerable<LocationModel> Products { get; private set; }

        public void OnGet()
        {
            Products = ProductService.GetAllData();
        }
    }
}
