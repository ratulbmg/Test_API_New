namespace Test_API_New.BusinessLogicLayer.DataTransferObject
{
    public class UserResponseDTO
    {
        // public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string Email { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double ProductPrice { get; set; }
    }

    public class UserRequestDTO
    {
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string Email { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public int ProductId { get; set; }
    }
}
