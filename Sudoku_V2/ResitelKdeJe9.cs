// pravidlo - v nekterych polickach jsou vepsany sipky
	// tyto sipky udavaji, jakym smerem od tohoto policka je v radku ci sloupci cislo 9
	// zaroven cislo v teto sipce udava, o kolik policek dal tato 9 je
// vstup - tabulka 9x9 nadepsana KdeJe9
	// sipky jsou oznaceny jako '>', '<', 'V' nebo 'A'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelKdeJe9 : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelKdeJe9(Sudoku sudoku) {
			// polickum s sipkou pripiseme vyznamny bonus
			this.HodnotaBonus = 1500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// pokud narazim na sipku
				// policku s sipkou dam odkaz na sebe sama s informaci o sipce ('A' ~ -1, 'V' ~ -2, '<' ~ -3, '>' ~ -4)
				// polickum ve smeru sipky dam odkaz na policko s sipkou, treti cislo udava jejich vzdalenost
				// polickum za sipkou zakazu cislo 9
				// policku s sipkou zakazu ta cisla, ktera pri vyplneni by ukazovala mimo mrizku
					// napr. na tretim radku sipka nahoru - na cisle sipky muze byt jen 1 nebo 2, jinak bychom se odkazovali mimo mrizku
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.KdeJe9[i][j] == "A") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 1, -1});
						for (int k = 0; k < i; k++) {
							this.Policka[k, j].Add(new int[] {i + 1, j + 1, i - k});
						}
						for (int k = i + 1; k < sudoku.RozmerMrizka; k++) {
							sudoku.BanujPolicko(k + 1, j + 1, 9);
						}
						for (int k = i + 1; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
					} else if (sudoku.Uloha.KdeJe9[i][j] == "V") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 1, -2});
						for (int k = i + 1; k < sudoku.RozmerMrizka; k++) {
							this.Policka[k, j].Add(new int[] {i + 1, j + 1, k - i});
						}
						for (int k = 0; k < i; k++) {
							sudoku.BanujPolicko(k + 1, j + 1, 9);
						}
						for (int k = sudoku.RozmerMrizka - i; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
					} else if (sudoku.Uloha.KdeJe9[i][j] == "<") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 1, -3});
						for (int k = 0; k < j; k++) {
							this.Policka[i, k].Add(new int[] {i + 1, j + 1, j - k});
						}
						for (int k = j + 1; k < sudoku.RozmerMrizka; k++) {
							sudoku.BanujPolicko(i + 1, k + 1, 9);
						}
						for (int k = j + 1; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
					} else if (sudoku.Uloha.KdeJe9[i][j] == ">") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 1, -4});
						for (int k = j + 1; k < sudoku.RozmerMrizka; k++) {
							this.Policka[i, k].Add(new int[] {i + 1, j + 1, k - j});
						}
						for (int k = 0; k < j; k++) {
							sudoku.BanujPolicko(i + 1, k + 1, 9);
						}
						for (int k = sudoku.RozmerMrizka - j; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
					}
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.KdeJe9, sudoku.RozmerMrizka, "Kde Je 9:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// pokud se na policka nachazi sipka (treti cislo je -1 az -4), pak cislo v danem smeru vzdalene odpovidajici pocet kroku je 9
			// dale musim projit vsechny sipky, ktere na policko ukazuji a
				// bud jim vepsat prislusne cislo (vzdalenost policka a sipky), pokud jsem 9
				// nebo zabanovat prislusne cislo, nejsem-li 9
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				if (policko[2] == -1) {
					sudoku.BanujPolickoInverse(radek - cislo, sloupec, 9);
				} else if (policko[2] == -2) {
					sudoku.BanujPolickoInverse(radek + cislo, sloupec, 9);
				} else if (policko[2] == -3) {
					sudoku.BanujPolickoInverse(radek, sloupec - cislo, 9);
				} else if (policko[2] == -4) {
					sudoku.BanujPolickoInverse(radek, sloupec + cislo, 9);
				} else if (cislo == 9) {
					sudoku.BanujPolickoInverse(policko[0], policko[1], policko[2]);
				} else {
					sudoku.BanujPolicko(policko[0], policko[1], policko[2]);
				}
			}
		}
	}
}
