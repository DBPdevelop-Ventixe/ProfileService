namespace WebApi.Services;

using Grpc.Core;
using WebApi;
using WebApi.Data;
using WebApi.Entity;

public class ProfileService(DataContext context) : ProtoProfileService.ProtoProfileServiceBase
{
    private readonly DataContext _context = context;


    // Create Profile
    public override async Task<AddProfileResponse> AddProfile(AddProfileRequest request, ServerCallContext context)
    {
        // Validate all inputs in the request
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.FirstName) ||
            string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Phone) ||
            string.IsNullOrEmpty(request.Street) || string.IsNullOrEmpty(request.ZipCode) ||
            string.IsNullOrEmpty(request.City)|| string.IsNullOrEmpty(request.Country))
        {
            return new AddProfileResponse { Success = false, Message = "All fields are required." };
        }

        // Check if the email already exists
        var existingProfile = await _context.Profiles.FindAsync(request.Userid);
        if (existingProfile != null)
            return new AddProfileResponse { Success = false, Message = "User already have an profile." };

        // Add the new profile
        try
        {
            var profile = new ProfileEntity
            {
                Id = request.Userid,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Street = request.Street,
                ZipCode = request.ZipCode,
                City = request.City,
                State = request.State ?? "",
                Country = request.Country
            };

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return new AddProfileResponse { Success = true, Message = "Profile was added successfully." };
        }
        catch (RpcException ex)
        {
            return new AddProfileResponse { Success = false, Message = $"Error while saving the profile: {ex}" };
        }
    }

    public override async Task<AddProfileResponse> AddEmptyProfile(AddEmptyProfileRequest request, ServerCallContext context)
    {
        // Validate all inputs in the request
        if (string.IsNullOrEmpty(request.UserId))
            return new AddProfileResponse { Success = false, Message = "User ID is required." };

        // Check if the email already exists
        var existingProfile = await _context.Profiles.FindAsync(request.UserId);
        if (existingProfile != null)
            return new AddProfileResponse { Success = false, Message = "User already have an profile." };

        // Add the new profile
        try
        {
            var profile = new ProfileEntity
            {
                Id = request.UserId,
                Email = request.UserId,
                ImageUrl = "../images/defaultmember.png"
            };
            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();
            return new AddProfileResponse { Success = true, Message = "Profile was added successfully." };
        }
        catch (RpcException ex)
        {
            return new AddProfileResponse { Success = false, Message = $"Error while saving the profile: {ex}" };
        }

    }


    // Update Profile
    public override async Task<UpdateProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
    {
        // Validate all inputs in the request
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.FirstName) ||
            string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Phone) ||
            string.IsNullOrEmpty(request.Street) || string.IsNullOrEmpty(request.ZipCode) ||
            string.IsNullOrEmpty(request.City) || string.IsNullOrEmpty(request.Country))
        {
            return new UpdateProfileResponse { Success = false, Message = "All fields are required." };
        }

        // Retrieve the existing profile
        var profile = await _context.Profiles.FindAsync(request.UserId);
        if (profile == null)
            return new UpdateProfileResponse { Success = false, Message = "User not found." };

        // Update the profile details
        try
        {
            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Email = request.Email;
            profile.Phone = request.Phone;
            profile.Street = request.Street;
            profile.ZipCode = request.ZipCode;
            profile.City = request.City;
            profile.State = request.State;
            profile.Country = request.Country;
            profile.ImageUrl = request.ImageUrl;
            var result = await _context.SaveChangesAsync();
            if(result > 0)
                return new UpdateProfileResponse { Success = true, Message = "Profile was updated successfully." };

            return new UpdateProfileResponse { Success = false, Message = "No changes were made to the profile." };
        }
        catch (RpcException ex)
        {
            return new UpdateProfileResponse { Success = false, Message = $"Error while updating the profile: {ex}" };
        }
    }

    // Get Profile by Email
    public override async Task<GetProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
    {
        // Validate the email input
        if (string.IsNullOrEmpty(request.UserId))
            return new GetProfileResponse { Success = false, Message = "Email is required." };

        // Retrieve the profile
        var profile = await _context.Profiles.FindAsync(request.UserId);
        if (profile == null)
            return new GetProfileResponse { Success = false, Message = "User not found." };

        // Return the profile details
        return new GetProfileResponse
        {
            Success = true,
            Message = "Profile retrieved successfully.",
            FirstName = profile.FirstName,
            LastName = profile.LastName,
            Email = profile.Email,
            Phone = profile.Phone,
            Street = profile.Street,
            ZipCode = profile.ZipCode,
            City = profile.City,
            State = profile.State,
            Country = profile.Country,
            ImageUrl = profile.ImageUrl
        };
    }
}
