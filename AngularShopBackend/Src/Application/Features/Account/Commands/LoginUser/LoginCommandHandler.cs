using Application.Contracts;
using Application.Dtos.Account;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Account.Commands.LoginUser;

public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public LoginCommandHandler(IMapper mapper, SignInManager<User> signInManager, UserManager<User> userManager,
        ITokenService tokenService)
    {
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                 .FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);


        if (user == null) throw new BadRequestEntityException("چنین کاربری یافت نشد لطفا ابتدا در سایت ثبت نام کنید");
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded) throw new BadRequestEntityException("نام کاربری یا رمز عبور اشتباه است");

        var mapUser = _mapper.Map<UserDto>(user);
        mapUser.Token = await _tokenService.CreateToken(user);
        return mapUser;
    }
}