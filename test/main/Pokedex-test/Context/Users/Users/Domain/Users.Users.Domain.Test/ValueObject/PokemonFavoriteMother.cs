﻿using System;
using System.Collections.Generic;
using System.Linq;
using Users.Users.Domain.Entities;
using Users.Users.Domain.ValueObject;

namespace Users.Users.Domain.Test.ValueObject
{
    public class PokemonFavoriteMother
    {
        private static string _pokemonName = "charizard";
        private static Random random = new Random();
        private const int NUM_OF_CHARS = 8;

        public static string Random()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, NUM_OF_CHARS)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string PokemonName()
        {
            return _pokemonName;
        }

        public static List<PokemonFavorite> PokemonFavorites()
        {
            return new List<PokemonFavorite>()
            {
                new PokemonFavorite() { PokemonName = "charizard" }
            };
        }
    }
}