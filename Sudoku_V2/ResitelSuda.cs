// pravidlo - na vyznacenych polich se mohou nachazet pouze suda cisla
// vstup - tabulka 9x9 nadepsana Suda
	// policka se sudymi cisly jsou vyznacena symbolem 'X'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelSuda : IResitel {
		public ResitelSuda(Sudoku sudoku) {
			// najdu-li ve vstupnim souboru znak 'X', pak zakazi v prislusnem policku licha cisla
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Suda[i][j] == "X") {
						for (int k = 1; k <= sudoku.RozmerMrizka; k += 2) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.Suda, sudoku.RozmerMrizka, "Suda:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
		// neni treba nic banovat, prace byla odvedena na pocatku pri iniciaci Resitele
		}
	}
}
