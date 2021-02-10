// pravidlo - podbarvena pole tvori pevnost
	// pokud podbarvene pole sousedi s nepodbarvenym, pak podbarvene pole je vetsi
// vstup - tabulka 9x9 nadepsana Pevnost
	// policka pevnosti maji vepsano 'X'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelPevnost : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelPevnost(Sudoku sudoku) {
			// polickum pevnosti a sousedicim pripiseme bonus
			this.HodnotaBonus = 500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// pokud narazime na policko pevnosti
				// zkontrolujeme 4 okolni pole, zda jsou nebo nejsou pevnosti
				// pokud sousedici pole neni pevnost
					// zapiseme jim vzajemne odkazy
					// treti cislo je bud 1 nebo -1
						// 1 znamena ja jsem pevnost a jsem tudiz vetsi, nez pole, na ktere se odkazuji
						// -1 zname opak
					// zaroven vime, ze policko pevnosti nemuze byt 1, sousedici pole nemuze byt 9
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Pevnost[i][j] == "X") {
						if (i + 1 < sudoku.RozmerMrizka && sudoku.Uloha.Pevnost[i + 1][j] == "") {
							this.Policka[i, j].Add(new int[]{i + 2, j + 1, 1});
							this.Policka[i + 1, j].Add(new int[]{i + 1, j + 1, -1});
							sudoku.BanujPolicko(i + 1, j + 1, 1);
							sudoku.BanujPolicko(i + 2, j + 1, sudoku.RozmerCisla);
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
							sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
						}
						if (i > 0 && sudoku.Uloha.Pevnost[i - 1][j] == "") {
							this.Policka[i, j].Add(new int[]{i, j + 1, 1});
							this.Policka[i - 1, j].Add(new int[]{i + 1, j + 1, -1});
							sudoku.BanujPolicko(i + 1, j + 1, 1);
							sudoku.BanujPolicko(i, j + 1, sudoku.RozmerCisla);
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
							sudoku.Policka[i - 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i - 1, j].HodnotaBonus);
						}
						if (j + 1 < sudoku.RozmerMrizka && sudoku.Uloha.Pevnost[i][j + 1] == "") {
							this.Policka[i, j].Add(new int[]{i + 1, j + 2, 1});
							this.Policka[i, j + 1].Add(new int[]{i + 1, j + 1, -1});
							sudoku.BanujPolicko(i + 1, j + 1, 1);
							sudoku.BanujPolicko(i + 1, j + 2, sudoku.RozmerCisla);
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
							sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
						}
						if (j > 0 && sudoku.Uloha.Pevnost[i][j - 1] == "") {
							this.Policka[i, j].Add(new int[]{i + 1, j, 1});
							this.Policka[i, j - 1].Add(new int[]{i + 1, j + 1, -1});
							sudoku.BanujPolicko(i + 1, j + 1, 1);
							sudoku.BanujPolicko(i + 1, j, sudoku.RozmerCisla);
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
							sudoku.Policka[i, j - 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j - 1].HodnotaBonus);
						}
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.Pevnost, sudoku.RozmerMrizka, "Pevnost:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// mam-li odkaz na jine policko (tudiz jsem pevnost a sousedim s polickem, jez pevnosti neni, nebo naopak)
				// dle toho, zda jsem mensi nebo vetsi nez policko, na ktere se odkazuji, tak zakazu vetsi ci mensi cisla v nem
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				if (policko[2] == 1) {
					for (int i = cislo + 1; i <= sudoku.RozmerMrizka; i++) {
						sudoku.BanujPolicko(policko[0], policko[1], i);
					}
				} else {
					for (int i = 1; i <= cislo - 1; i++) {
						sudoku.BanujPolicko(policko[0], policko[1], i);
					}
				}
			}
		}
	}
}
