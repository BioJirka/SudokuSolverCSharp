// pravidlo - mezi nekterymi dvojicemi sousedicich cisel je vyznacen jejich rozdil
// vstup - tabulka 9x8 nadepsana Rozdil_Radek a tabulka 8x9 nadepsana Rozdil_Sloupec
	// cisla v nich udavaji rozdil prislusnych dvojic poli

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelRozdil : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelRozdil(Sudoku sudoku) {
			// polickum jejichz soucet je vyznacen pripiseme vyznamny bonus
			this.HodnotaBonus = 1500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// je-li v radku zobrazen rozdil dvou sousedicich policek
				// policka si na sebe vzajemne odkazeme, treti cislo bude jejich rozdil
				// je-li rozdil prilis velky, musime banovat cisla ve stredu rozsahu
					// napr. je-li rozdil 6, pak nemuzeme mit cisla 4,5,6, protoze se k nim nenajde vhodny protejsek
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka - 1; j++) {
					if (sudoku.Uloha.Rozdil_Radek[i][j] != "") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 2, Convert.ToInt32(sudoku.Uloha.Rozdil_Radek[i][j])});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, Convert.ToInt32(sudoku.Uloha.Rozdil_Radek[i][j])});
						if (2 * Convert.ToInt32(sudoku.Uloha.Rozdil_Radek[i][j]) > sudoku.RozmerCisla) {
							for (int k = sudoku.RozmerCisla - Convert.ToInt32(sudoku.Uloha.Rozdil_Radek[i][j]) + 1; k <= Convert.ToInt32(sudoku.Uloha.Rozdil_Radek[i][j]); k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 1, j + 2, k);
							}
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
					}
				}
			}
			// totez pro sloupce
			for (int i = 0; i < sudoku.RozmerMrizka - 1; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Rozdil_Sloupec[i][j] != "") {
						this.Policka[i, j].Add(new int[] {i + 2, j + 1, Convert.ToInt32(sudoku.Uloha.Rozdil_Sloupec[i][j])});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, Convert.ToInt32(sudoku.Uloha.Rozdil_Sloupec[i][j])});
						if (2 * Convert.ToInt32(sudoku.Uloha.Rozdil_Sloupec[i][j]) > sudoku.RozmerCisla) {
							for (int k = sudoku.RozmerCisla - Convert.ToInt32(sudoku.Uloha.Rozdil_Sloupec[i][j]) + 1; k <= Convert.ToInt32(sudoku.Uloha.Rozdil_Sloupec[i][j]); k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 2, j + 1, k);
							}
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniRadek(sudoku.Uloha.Rozdil_Radek, sudoku.RozmerMrizka, "Rozdil radek:");
			FunkceNapisZadani.NapisZadaniSloupec(sudoku.Uloha.Rozdil_Sloupec, sudoku.RozmerMrizka, "Rozdil sloupec:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// je-li policko soucasti dvojice, u ktere je vyznacen rozdil, pak sousedovi musime zabanovat cisla nedavajici potrebny rozdil
			foreach (var policko in Policka[radek - 1, sloupec - 1]) {
				if (cislo <= policko[2]) {
					sudoku.BanujPolickoInverse(policko[0], policko[1], cislo + policko[2]);
				} else if (cislo + policko[2] > sudoku.RozmerCisla) {
					sudoku.BanujPolickoInverse(policko[0], policko[1], cislo - policko[2]);
				} else {
					sudoku.BanujPolickoInverse_2(policko[0], policko[1], cislo - policko[2], cislo + policko[2]);
				}
			}
		}
	}
}
