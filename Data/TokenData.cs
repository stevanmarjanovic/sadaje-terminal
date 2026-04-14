using System.Collections.Generic;
using SadaJeTerminal.Domain;

namespace SadaJeTerminal.Data;

// TODO Move tags to ENUM with explanations

public class TokenData
{
     public static List<List<Token>> Tokens = [
           [
                new Token { Value = "CX" },
                new Token { Value = "SADA", Tags = [ "NOW" ] },
                new Token { Value = "L" },
                new Token { Value = "JE", Tags = ["NOW"] },
                new Token { Value = "AKL" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "P2", "P20", "P25" ] },
                new Token { Value = "DESET", Tags = [ "P10", "P20", "P25" ] },
                new Token { Value = "POLA", Tags = [ "HALF" ] }
            ],
            [
                new Token { Value = "R" },
                new Token { Value = "PET", Tags = [ "P5", "P15", "P25" ] },
                new Token { Value = "NAEST", Tags = [ "P15" ] },
                new Token { Value = "S" },
                new Token { Value = "DO", Tags = [ "UNTIL" ] },
            ],
            [
                new Token { Value = "G" },
                new Token { Value = "JEDAN", Tags = [ "C1", "C11" ] },
                new Token { Value = "AES", Tags = [ "C11" ] },
                new Token { Value = "T", Tags = [ "C3", "C11" ] },
                new Token { Value = "RI", Tags = [ "C3" ] },
            ],
            [
                new Token { Value = "SEDAM", Tags = [ "C7" ] },
                new Token { Value = "ČETIRI", Tags = [ "C4" ] },
                new Token { Value = "H" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "C2", "C12" ] },
                new Token { Value = "NAEST", Tags = [ "C12" ] },
                new Token { Value = "ŠEST", Tags = [ "C6" ] },
            ],
            [
                new Token { Value = "DEVET", Tags = [ "C9" ] },
                new Token { Value = "DE", Tags = [ "C10" ] },
                new Token { Value = "S", Tags = [ "C10", "HOURGS" ] },
                new Token { Value = "ET", Tags = [ "C10" ] },
                new Token { Value = "BM" },
            ],
            [
                new Token { Value = "F" },
                new Token { Value = "PET", Tags = [ "C5" ] },
                new Token { Value = "Z" },
                new Token { Value = "OS", Tags = [ "C8" ] },
                new Token { Value = "A", Tags = [ "C8", "HOURGS" ] },
                new Token { Value = "M", Tags = [ "C8" ] },
                new Token { Value = "A" },
                new Token { Value = "I", Tags = [ "AND" ] },
                new Token { Value = "S" },
            ],
            [
                new Token { Value = "DVA", Tags = [ "S2", "S20", "S25" ] },
                new Token { Value = "DESE", Tags = [ "S10", "S20", "S25" ] },
                new Token { Value = "T", Tags = [ "S10", "S20", "S25", "HOURGS" ] },
                new Token { Value = "SAT", Tags = [ "HOURGP", "HOURNS" ] },
                new Token { Value = "I", Tags = [ "HOURGP" ] },
            ],
            [
                new Token { Value = "RSV" },
                new Token { Value = "PET", Tags = [ "S5", "S15", "S25" ] },
                new Token { Value = "N", Tags = [ "S15" ] },
                new Token { Value = "A", Tags = [ "S15", "HOURGS" ] },
                new Token { Value = "EST", Tags = [ "S15" ] },
                new Token { Value = "L" },
            ]
        ];
}