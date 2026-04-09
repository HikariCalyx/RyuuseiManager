using System;
using System.Collections.Generic;
using System.Text;

namespace RyuuseiManager
{
    public class GameID
    {
        public static readonly Dictionary<int, List<string>> ExpectedImportSources = new Dictionary<int, List<string>>()
        {
            { 10, ["data010Slot.bin"] },
            { 11, ["data011Slot.bin"] },
            { 12, ["data012Slot.bin"] },
            { 20, ["data020Slot.bin"] },
            { 21, ["data021Slot.bin"] },
            { 22, ["data022Slot.bin"] },
            { 23, ["data023Slot.bin"] },
            { 30, ["data030Slot.bin", "data031Slot.bin"] },
            { 31, ["data030Slot.bin", "data031Slot.bin"] },
            { 32, ["data032Slot.bin", "data033Slot.bin"] },
            { 33, ["data032Slot.bin", "data033Slot.bin"] },
        };
    }
}
