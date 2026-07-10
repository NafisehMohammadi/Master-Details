namespace MasterDetails.ApplicationServices.Dtos.CustomerDto
{
    public class GetCustomerDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }
    }
}
