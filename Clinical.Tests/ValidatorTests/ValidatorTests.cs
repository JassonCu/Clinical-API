using Clinical.UseCases.UseCases.Auth.Commands.ChangePasswordCommand;
using Clinical.UseCases.UseCases.Auth.Commands.LoginCommand;
using Clinical.UseCases.UseCases.Auth.Commands.RegisterCommand;
using Clinical.UseCases.UseCases.Patient.Commands.CreateCommand;

namespace Clinical.Test.ValidatorTests
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator _validator = new();

        [Fact]
        public void Valid_When_UsernameAndPassword_Provided()
            => Assert.True(_validator.Validate(new LoginCommand { Username = "alice", Password = "x" }).IsValid);

        [Theory]
        [InlineData(null, "x")]
        [InlineData("", "x")]
        [InlineData("alice", "")]
        public void Invalid_When_FieldMissing(string? username, string password)
            => Assert.False(_validator.Validate(new LoginCommand { Username = username, Password = password }).IsValid);
    }

    public class ChangePasswordValidatorTests
    {
        private readonly ChangePasswordValidator _validator = new();

        [Fact]
        public void Valid_When_CurrentSet_And_NewIsStrongAndDifferent()
            => Assert.True(_validator.Validate(new ChangePasswordCommand
            {
                CurrentPassword = "OldPass1!",
                NewPassword = "BrandNew123!"
            }).IsValid);

        [Fact]
        public void Invalid_When_CurrentPassword_Empty()
            => Assert.False(_validator.Validate(new ChangePasswordCommand
            {
                CurrentPassword = "",
                NewPassword = "BrandNew123!"
            }).IsValid);

        [Fact]
        public void Invalid_When_NewPassword_TooShort()
            => Assert.False(_validator.Validate(new ChangePasswordCommand
            {
                CurrentPassword = "OldPass1!",
                NewPassword = "short"
            }).IsValid);

        [Fact]
        public void Invalid_When_NewPassword_EqualsCurrent()
            => Assert.False(_validator.Validate(new ChangePasswordCommand
            {
                CurrentPassword = "SamePass123!",
                NewPassword = "SamePass123!"
            }).IsValid);
    }

    public class RegisterValidatorTests
    {
        private readonly RegisterValidator _validator = new();

        private static RegisterCommand Valid() => new()
        {
            Username = "newuser",
            Email = "user@clinic.test",
            Password = "Passw0rd!",
            RoleId = 2
        };

        [Fact]
        public void Valid_Command_Passes() => Assert.True(_validator.Validate(Valid()).IsValid);

        [Theory]
        [InlineData("password")]   // no upper, no digit, no special
        [InlineData("Password")]   // no digit, no special
        [InlineData("Password1")]  // no special
        [InlineData("Pass1!")]     // too short
        public void Invalid_When_PasswordWeak(string password)
        {
            var cmd = Valid();
            cmd.Password = password;
            Assert.False(_validator.Validate(cmd).IsValid);
        }

        [Fact]
        public void Invalid_When_Username_TooShort()
        {
            var cmd = Valid();
            cmd.Username = "ab";
            Assert.False(_validator.Validate(cmd).IsValid);
        }
    }

    public class CreatePatientValidatorTests
    {
        private readonly CreatePatientValidator _validator = new();

        private static CreatePatientCommand Valid() => new()
        {
            DocumentNumber = "12345678",
            FirstName = "Ana",
            LastName = "Lopez"
        };

        [Fact]
        public void Valid_Command_Passes() => Assert.True(_validator.Validate(Valid()).IsValid);

        [Fact]
        public void Invalid_When_DocumentNumber_Missing()
        {
            var cmd = Valid();
            cmd.DocumentNumber = "";
            Assert.False(_validator.Validate(cmd).IsValid);
        }

        [Fact]
        public void Invalid_When_Gender_NotAllowed()
        {
            var cmd = Valid();
            cmd.Gender = "X";
            Assert.False(_validator.Validate(cmd).IsValid);
        }
    }
}
