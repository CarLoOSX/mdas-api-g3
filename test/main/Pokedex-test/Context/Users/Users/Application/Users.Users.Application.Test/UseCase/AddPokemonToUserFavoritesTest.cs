using Moq;
using System.Threading.Tasks;
using Users.Users.Application.UseCase;
using Users.Users.Domain.Aggregate;
using Users.Users.Domain.Repositories;
using Users.Users.Domain.Services;
using Users.Users.Domain.Test.Aggregate;
using Users.Users.Domain.Test.ValueObject;
using Users.Users.Domain.ValueObject;
using Xunit;

namespace Users.Users.Application.Test.UseCase
{
    public class AddPokemonToUserFavoritesTest
    {
        [Fact]
        public async Task AddPokemonToUserFavorites_ReturnsVoid()
        {
            #region Arrange
            int pokemonId = PokemonIdMother.Id();
            string userId = UserIdMother.Id();
            var userRepository = new Mock<UserRepository>();

            userRepository
                .Setup(r => r.Find(It.IsAny<UserId>()))
                .ReturnsAsync(UserMother.User(userId));

            userRepository
                .Setup(r => r.Exists(It.IsAny<UserId>()))
                .ReturnsAsync(true);

            userRepository
                .Setup(r => r.SaveFavorites(It.IsAny<User>()));

            UserFinder userFinder = new UserFinder(userRepository.Object);
            PokemonFavoriteCreator pokemonFavoriteCreator = new PokemonFavoriteCreator(userRepository.Object);
            AddPokemonToUserFavorites addPokemonToUserFavorites = new AddPokemonToUserFavorites(userFinder, pokemonFavoriteCreator);

            #endregion

            #region Act
            await addPokemonToUserFavorites.Execute(userId, pokemonId);

            #endregion

            #region Assert
            userRepository.Verify(r => r.SaveFavorites(It.IsAny<User>()), Times.Once());
            #endregion
        }

        [Fact]
        public void AddPokemonToUserFavorites_ReturnsUserNotFoundException()
        {
            #region Arrange
            int pokemonId = PokemonIdMother.Id();
            string userId = UserIdMother.Id();
            string expectedMessage = $"User '{userId}' does not exists";

            var userRepository = new Mock<UserRepository>();

            userRepository
                .Setup(r => r.SaveFavorites(It.IsAny<User>()));

            UserFinder userFinder = new UserFinder(userRepository.Object);
            PokemonFavoriteCreator pokemonFavoriteCreator = new PokemonFavoriteCreator(userRepository.Object);
            AddPokemonToUserFavorites addPokemonToUserFavorites = new AddPokemonToUserFavorites(userFinder, pokemonFavoriteCreator);

            #endregion

            #region Act
            var exception = Record.ExceptionAsync(async () => await addPokemonToUserFavorites.Execute(userId, pokemonId));

            #endregion

            #region Assert
            Assert.Equal(expectedMessage, exception.Result.Message);

            #endregion
        }

        [Fact]
        public void AddPokemonToUserFavorites_ReturnsPokemonAlreadyExistsException()
        {
            #region Arrange
            int pokemonId = PokemonIdMother.Id();
            string userId = UserIdMother.Id();
            string expectedMessage = $"The pokemon with Id '{pokemonId}' already exists in user favorites list";

            var userRepository = new Mock<UserRepository>();

            userRepository
                .Setup(r => r.Find(It.IsAny<UserId>()))
                .ReturnsAsync(UserMother.UserWithFavorites(userId, pokemonId));

            userRepository
                .Setup(r => r.Exists(It.IsAny<UserId>()))
                .ReturnsAsync(true);

            userRepository
                .Setup(r => r.SaveFavorites(It.IsAny<User>()));

            UserFinder userFinder = new UserFinder(userRepository.Object);
            PokemonFavoriteCreator pokemonFavoriteCreator = new PokemonFavoriteCreator(userRepository.Object);
            AddPokemonToUserFavorites addPokemonToUserFavorites = new AddPokemonToUserFavorites(userFinder, pokemonFavoriteCreator);

            #endregion

            #region Act
            var exception = Record.ExceptionAsync(async () => await addPokemonToUserFavorites.Execute(userId, pokemonId));

            #endregion

            #region Assert
            Assert.Equal(expectedMessage, exception.Result.Message);

            #endregion
        }
    }
}
