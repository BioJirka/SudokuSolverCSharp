// pravidlo - symbolem jsou vyznaceny vsechny dvojice sousedicich cisel, kde tato cisla jsou po sobe jdouci
// vstup - tabulka 9x8 nadepsana Sousledna_Radek a tabulka 8x9 nadepsana Sousledna_Sloupec
	// dvojice cisel, ktere jsou po sobe jdouci jsou oznaceny 'X'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelSousledna : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelSousledna(Sudoku sudoku) {
			// je-li policko soucasti dvojice po sobe jdoucich cisel, je mu pripsan vyznamny bonus
			this.HodnotaBonus = 1500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// jsou-li cisla v radku po sobe jdouci (vyznaceno 'X'), pak jim zapiseme vzajemny odkaz s tretim cislem 1
			// nejsou-li po sobe jdouci, pak jim tento odkaz zapiseme presto a treti cislo bude 0
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka - 1; j++) {
					if (sudoku.Uloha.Sousledna_Radek[i][j] == "X") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 2, 1});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, 1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
					} else {
						this.Policka[i, j].Add(new int[] {i + 1, j + 2, 0});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, 0});
					}
				}
			}
			// totez pro sloupce
				for (int i = 0; i < sudoku.RozmerMrizka - 1; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Sousledna_Sloupec[i][j] == "X") {
						this.Policka[i, j].Add(new int[] {i + 2, j + 1, 1});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, 1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
					} else {
						this.Policka[i, j].Add(new int[] {i + 2, j + 1, 0});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, 0});
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniRadek(sudoku.Uloha.Sousledna_Radek, sudoku.RozmerMrizka, "Sousledna radek:");
			FunkceNapisZadani.NapisZadaniSloupec(sudoku.Uloha.Sousledna_Sloupec, sudoku.RozmerMrizka, "Sousledna sloupec:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// pokud ma policko souslednou dvojici, tato musi byt cislo - 1 nebo cislo + 1
			// pokud sousedici policko neni sousledne, musime cislo - 1 a cislo + 1 banovat
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				if (policko[2] == 1) {
					if (cislo == 1) {
						sudoku.BanujPolickoInverse(policko[0], policko[1], cislo + 1);
					} else if (cislo == sudoku.RozmerCisla) {
						sudoku.BanujPolickoInverse(policko[0], policko[1], cislo - 1);
					} else {
						sudoku.BanujPolickoInverse_2(policko[0], policko[1], cislo - 1, cislo + 1);
					}
				} else {
					if (cislo > 1) {
						sudoku.BanujPolicko(policko[0], policko[1], cislo - 1);
					}
					if (cislo < sudoku.RozmerCisla) {
						sudoku.BanujPolicko(policko[0], policko[1], cislo + 1);
					}
				}
			}
		}
	}
}
