using Core.DTOs;
using Core.Entities;

namespace Core.Mappings
{
    public static class MappingExtensions
    {
        public static MovieDto ToDto(this Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                TmdbId = movie.TmdbId,
                Title = movie.Title,
                OriginalTitle = movie.OriginalTitle,
                Overview = movie.Overview,
                PosterPath = movie.PosterPath,
                BackdropPath = movie.BackdropPath,
                ReleaseDate = movie.ReleaseDate,
                VoteAverage = movie.VoteAverage,
                Runtime = movie.Runtime,
                WatchedDate = movie.WatchedDate,
                UserRating = movie.UserRating,
                UserNotes = movie.UserNotes,
                CreatedAt = movie.CreatedAt,
                Genres = movie.Genres,
                Username = movie.User?.Username ?? "Unknown"
            };
        }

        public static Movie ToEntity(this CreateMovieDto dto)
        {
            return new Movie
            {
                TmdbId = dto.TmdbId,
                Title = dto.Title,
                OriginalTitle = dto.OriginalTitle,
                Overview = dto.Overview,
                PosterPath = dto.PosterPath,
                BackdropPath = dto.BackdropPath,
                ReleaseDate = dto.ReleaseDate,
                VoteAverage = dto.VoteAverage,
                Runtime = dto.Runtime,
                WatchedDate = dto.WatchedDate,
                UserRating = dto.UserRating,
                UserNotes = dto.UserNotes,
                Genres = dto.Genres,
                UserId = dto.UserId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                FavoriteTmdbId = user.FavoriteTmdbId,
                ProfilePhoto = user.ProfilePhoto  
            };
        }
    }
}