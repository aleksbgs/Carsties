using AuctionService.Entities;

namespace AuctionService.UnitTest;

public class AuctionEntityTests
{
    [Fact]
    public void HasReservePrice_ReservePriceGtZero_True()
    {
        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            ReservePrice = 10
        };

        var result = auction.HasReservedPrice();
        Assert.True(result);
        
    }
  
    
}