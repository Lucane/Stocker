﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace StockerDB.Data.Stocker
{
    public partial class PartsList
    {
        public int Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public string PartNumber { get; set; }
        public string PartDescription { get; set; }
        public string PartCategory { get; set; }
        public string PartImages { get; set; }
        public string ModelType { get; set; }
        public string ModelName { get; set; }
    }
}