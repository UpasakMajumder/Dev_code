﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.TemplatedProduct.Responses
{
    public class ProductTemplatesDTO
    {
        public string Title { get; set; }
        public string OpenInDesignBtn { get; set; }
        public  ProductTemplatesHeaderDTO[] Header { get; set; }
        public ProductTemplateDTO[] Data { get; set; }
    }
}
