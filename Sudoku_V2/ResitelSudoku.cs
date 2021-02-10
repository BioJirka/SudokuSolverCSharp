// zakladni trida pouzita vzdy
// hlida, aby na jednom radku ci sloupci nabyly dve stejna cisla

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelSudoku : IResitel {
		public ResitelSudoku(Sudoku sudoku) {
		}
		public void NapisZadani(Sudoku sudoku) {
			Console.WriteLine("Sudoku zadani:");
			Console.Write("Typ: ");
			foreach (var typ in sudoku.Uloha.Typ) {
				Console.Write(typ + " ");
			}
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.Cisla, sudoku.RozmerMrizka, "");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// zadane cislo zakaze v celem radku a sloupci
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				sudoku.BanujPolicko(radek, i + 1, cislo);
				sudoku.BanujPolicko(i + 1, sloupec, cislo);
			}
		}
	}
}
