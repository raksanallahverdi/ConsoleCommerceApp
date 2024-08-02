namespace Application.Services.Abstract
{
    public interface IAdminService
    {
        public void GetSellers() { }
        public void CreateSeller() { }
        public void DeleteSeller() { }
        public void GetCustomers() { }
        public void CreateCustomer() { }
        public void DeleteCustomer() { }
        public void AddCategory() { }
        public void SeeOrders() { }
        public void SeeOrderBySeller() { }
        public void SeeOrderByCustomer() { }
        public void SeeOrderByDate() { }
    }
}
