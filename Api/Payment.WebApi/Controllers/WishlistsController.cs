using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.BusinessLayer.Abstract;
using Payment.DtoLayer.Dtos.WishlistDtos;
using Payment.EntityLayer.Concrete;


namespace Payment.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistsController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    private readonly IMapper _mapper;

    public WishlistsController(IWishlistService wishlistService, IMapper mapper)
    {
        _wishlistService = wishlistService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWishlists()
    {
        var values = await _wishlistService.GetListAsync();
        var wishlists = _mapper.Map<List<ResultWishlistDto>>(values);
        return Ok(wishlists);
    }

    [HttpGet("GetWishlistByUserId")]
    public async Task<IActionResult> GetWishlistByUserId(int userId)
    {
        var values = await _wishlistService.TGetWishlistByUserId(userId);
        var wishlists = _mapper.Map<List<ResultWishlistDto>>(values);
        return Ok(wishlists);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWishlist(int id)
    {
        var value = await _wishlistService.GetByIdAsync(id);
        if (value == null)
        {
            return NotFound("Wishlist bulunamadı.");
        }
        var wishlist = _mapper.Map<GetByIdWishlistDto>(value);
        return Ok(wishlist);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateWishlist(CreateWishlistDto createWishlistDto)
    {
        var wishlist = _mapper.Map<Wishlist>(createWishlistDto);
        await _wishlistService.InsertAsync(wishlist);
        return Ok("Wishlist oluşturuldu.");
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateWishlist(UpdateWishlistDto updateWishlistDto)
    {
        var existingWishlist = await _wishlistService.GetByIdAsync(updateWishlistDto.Id);
        if (existingWishlist == null)
        {
            return NotFound("Wishlist bulunamadı.");
        }
        var wishlist = _mapper.Map<Wishlist>(updateWishlistDto);
        await _wishlistService.UpdateAsync(wishlist);
        return Ok("Wishlist güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWishlist(int id)
    {
        var existingWishlist = await _wishlistService.GetByIdAsync(id);
        if (existingWishlist == null)
        {
            return NotFound("Wishlist bulunamadı.");
        }
        await _wishlistService.DeleteAsync(existingWishlist);
        return Ok("Wishlist silindi.");
    }
}
