using _1_API.Request;
using _2_Domain;
using _3_Data;
using _3_Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace _1_API.Controllers;

[Route("api/v1/user/")]
[ApiController]
public class UserRegisterController : ControllerBase
{
    //  @Dependencies
    private readonly IMapper _mapper;
    private readonly IUserRegisterDomain _userRegisterDomain;

    //  @Constructor
    public UserRegisterController(
        IUserRegisterDomain userRegisterDomain,
        IMapper mapper
    )
    {
        this._userRegisterDomain = userRegisterDomain;
        this._mapper = mapper;
    }
        
    
    
    /// <summary>
    ///     Register a new user into the system.
    /// </summary>
    /// <param name="request">Body request parameters that represents the basic information of a user.</param>
    /// <returns>
    ///     Returns a message confirming the new user registered.
    /// </returns>
    /// <remarks>
    ///     This endpoint allows the creation of new users providing basic information such as user information and credentials.
    ///     The parameter type of this endpoint is an instance of <c>UserRegistrationRequest</c>.
    ///     <para>Here is an overview of the parameters the makes <c>UserRegistrationRequest</c>: </para>
    ///         <para> &#149; <b>Username</b>: The username of the user. </para>
    ///         <para> &#149; <b>Password</b>: The password. </para>
    ///         <para> &#149; <b>Email</b>: The email of the user. </para>
    ///         <para> &#149; <b>PhoneNumber</b>: The phone number. </para>
    ///     <para>You may be wondering where are the two segments we focus on. Well, an account allows any user to</para>
    ///     <para>find, buy real states and also allows users to create publications. We wrapped them up in a single account</para>
    ///     <para>to develop easier account creations and management.</para>
    /// </remarks>
    /// <response code="200">Returns <b>a confirmation message</b> for the new user registered.</response>
    /// <response code="500"><b>Something wrong</b> appears to be with your query.</response>
    /// <response code="400">You <b>didn't provide correct information</b> for the creation of a new user.</response>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> UserRegistration([FromBody] UserRegistrationRequest request)
    {
        if (!this.ModelState.IsValid)
        {
            return BadRequest();
        }
            
        var user = this._mapper.Map<UserRegistrationRequest, User>(request);
        var generatedId = await this._userRegisterDomain.RegisterUserAsync(user);
        if (generatedId > 0)
        {
            return StatusCode(
                StatusCodes.Status200OK,
                "User created successfully with id: " + generatedId
            );
        }
        return BadRequest("User has been generated with an invalid id... Something went wrong.");
    }
}