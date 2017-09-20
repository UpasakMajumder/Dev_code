﻿namespace Kadena.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
        public bool Checked { get; set; }
        public bool HasInput { get; set; }
        public string InputPlaceholder { get; set; }
        public string ClassName { get; set; }
        public bool IsUnpayable { get; set; }
    }
}