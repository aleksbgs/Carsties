using AuctionService.Controllers;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.RequestHelpers;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AuctionService.UnitTest;

public class AuctionControllerTests
{

    private readonly Mock<IAuctionRepository> _auctionRepo;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Fixture _fixture;
    private readonly AuctionController _controller;
    private readonly IMapper _mapper;




    public AuctionControllerTests()
    {
        _fixture = new Fixture();
        _auctionRepo = new Mock<IAuctionRepository>();
        _publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc => { mc.AddMaps(typeof(MappingProfiles).Assembly); }).CreateMapper()
            .ConfigurationProvider;

        _mapper = new Mapper(mockMapper);

        _controller = new AuctionController(_auctionRepo.Object, _mapper, _publishEndpoint.Object);
    }

    [Fact]
    public async Task GetAuctions_WitNoParamas_Returns10Auctions()
    {
        var auctions = _fixture.CreateMany<AuctionDto>(10).ToList();
        _auctionRepo.Setup(repo => repo.GetAuctionsAsync(null)).ReturnsAsync(auctions);

        var result = await _controller.GetAllAuctions(null);

        Assert.Equal(10, result.Value.Count);
        Assert.IsType<ActionResult<List<AuctionDto>>>(result);
    }

    [Fact]
    public async Task GetAuctionsById_WithValidGuid_ReturnsAuctions()
    {

        var auction = _fixture.Create<AuctionDto>();

        _auctionRepo.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<Guid>())).ReturnsAsync(auction);

        var result = await _controller.GetAuctionById(auction.Id);

        Assert.Equal(auction.Make, result.Value.Make);

        Assert.IsType<ActionResult<AuctionDto>>(result);

    }

    [Fact]
    public async Task GetAuctionsById_WithInvalidGuid_ReturnsNotFound()
    {

        _auctionRepo.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<Guid>())).ReturnsAsync(value: null);

        var result = await _controller.GetAuctionById(Guid.NewGuid());


        Assert.IsType<NotFoundResult>(result.Result);

    }

}