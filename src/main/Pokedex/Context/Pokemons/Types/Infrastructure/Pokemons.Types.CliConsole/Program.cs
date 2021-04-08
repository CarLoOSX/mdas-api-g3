﻿using Pokemons.Types.Application.Request;
using Pokemons.Types.Application.UseCase;
using Pokemons.Types.CliConsole.Converter;
using Pokemons.Types.Domain.Exceptions;
using Pokemons.Types.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                var response = await getPokemonType.Execute(
                    new GetPokemonTypeRequest()
                    {
                        PokemonName = pokemonName
                    });

                Console.WriteLine(StringConverter.Execute(response.Types));
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