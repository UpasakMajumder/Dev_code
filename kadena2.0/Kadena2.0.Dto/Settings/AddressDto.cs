namespace Kadena.Dto.Settings
{
    class AddressDto
    {
        public int Id { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool IsEditButton { get; set; }
        public bool IsRemoveButton { get; set; }
    }
}
