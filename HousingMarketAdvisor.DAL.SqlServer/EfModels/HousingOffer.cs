namespace HousingMarketAdvisor.DAL.SqlServer.EfModels;

public class HousingOffer
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public string Contact { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string ImageUrl { get; set; }
    public string ImageUrl2 { get; set; }
    public string ImageUrl3 { get; set; }
}