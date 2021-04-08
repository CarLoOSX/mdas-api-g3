﻿using Pokemons.Types.Application.UseCase;
using Pokemons.Types.CliConsole.Converter;
using Pokemons.Types.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Pokemons.Types.Persistence;
using System.Collections.Generic;
using Pokemons.Types.Domain.ValueObject;

namespace Pokemons.Types.CliConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string pokemonName;
            if (args.Any())
            {
                pokemonName = args.First();
            }
            else
            {
                Console.WriteLine("Enter pokemon name:");
                pokemonName = Console.ReadLine();
            }

            try
            {
                PokeApiPokemonTypeRepository pokeApiPokemonTypeRepository = new PokeApiPokemonTypeRepository();
                GetPokemonType getPokemonType = new GetPokemonType(pokeApiPokemonTypeRepository);
                IEnumerable<PokemonType> pokemonTypes = await getPokemonType.Execute(pokemonName);

                Console.WriteLine(PokemonTypeToStringConverter.Execute(pokemonTypes));
            }
            catch (PokemonNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
