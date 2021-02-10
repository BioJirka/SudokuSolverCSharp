// pravidlo - na vyznacenych polich se mohou nachazet pouze licha cisla
// vstup - tabulka 9x9 nadepsana Licha
	// policka s lichymi cisly jsou vyznacena symbolem 'X'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelLicha : IResitel {
		public ResitelLicha(Sudoku sudoku) {
			// najdu-li ve vstupnim souboru znak 'X', pak zakazi v prislusnem policku suda cisla
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Licha[i][j] == "X") {
						for (int k = 2; k <= sudoku.RozmerMrizka; k += 2) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.Licha, sudoku.RozmerMrizka, "Licha:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
		// neni treba nic banovat, prace byla odvedena na pocatku pri iniciaci Resitele
		}
	}
}
