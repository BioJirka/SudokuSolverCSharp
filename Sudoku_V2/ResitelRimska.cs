// pravidlo - symboly 'V' nebo 'X' jsou vyznaceny vsechny dvojice sousedicich cisel, jejichz soucet je 5 nebo 10
// vstup - tabulka 9x8 nadepsana Rimska_Radek a tabulka 8x9 nadepsana Rimska_sloupec
	// soucty 5 a 10 vyznaceny 'V' a 'X'

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelRimska : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelRimska(Sudoku sudoku) {
			// polickum ktere se nachazeji v nejake dvojici se souctem 5 ci 10 pripiseme vyznamny bonus
			this.HodnotaBonus = 1500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// pokud je v radku soucet sousedici dvojice 10
				// policka si na sebe vzajemne odkazeme, treti cislo bude 10
				// zadne z cisel nemuze byt 5
				// zadne z cisel nemuze byt 10 nebo vice (napr. kdybychom meli vetsi sudoku nez 9x9)
				// je-li rozmer mrizky mensi nez 9x9, pak musime zakazat mala cisla, aby soucet mohl dosahnout 10
			// pokud je tento soucet 5
				// policka si na sebe vzajemne odkazeme, treti cislo bude 5
				// zadne z cisel nemuze byt 5 nebo vice
				// je-li rozmer mrizky mensi nez 4x4, pak musime zakazat mala cisla, aby soucet mohl dosahnout 5
			// jinak si policka na sebe presto odkazeme, treti cislo bude 0
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka - 1; j++) {
					if (sudoku.Uloha.Rimska_Radek[i][j] == "X") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 2, 10});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, 10});
						sudoku.BanujPolicko(i + 1, j + 1, 5);
						sudoku.BanujPolicko(i + 1, j + 2, 5);
						for (int k = 10; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
							sudoku.BanujPolicko(i + 1, j + 2, k);
						}
						if (sudoku.RozmerCisla < 9) {
							for (int k = 1; k < 10 - sudoku.RozmerCisla; k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 1, j + 2, k);
							}
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
					} else if (sudoku.Uloha.Rimska_Radek[i][j] == "V") {
						this.Policka[i, j].Add(new int[] {i + 1, j + 2, 5});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, 5});
						for (int k = 5; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
							sudoku.BanujPolicko(i + 1, j + 2, k);
						}
						if (sudoku.RozmerCisla < 4) {
							for (int k = 1; k < 5 - sudoku.RozmerCisla; k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 1, j + 2, k);
							}
						}
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
					if (sudoku.Uloha.Rimska_Sloupec[i][j] == "X") {
						this.Policka[i, j].Add(new int[] {i + 2, j + 1, 10});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, 10});
						sudoku.BanujPolicko(i + 1, j + 1, 5);
						sudoku.BanujPolicko(i + 2, j + 1, 5);
						for (int k = 10; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
							sudoku.BanujPolicko(i + 2, j + 1, k);
						}
						if (sudoku.RozmerCisla < 9) {
							for (int k = 1; k < 10 - sudoku.RozmerCisla; k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 2, j + 1, k);
							}
						}
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
					} else if (sudoku.Uloha.Rimska_Sloupec[i][j] == "V") {
						this.Policka[i, j].Add(new int[] {i + 2, j + 1, 5});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, 5});
						for (int k = 5; k <= sudoku.RozmerCisla; k++) {
							sudoku.BanujPolicko(i + 1, j + 1, k);
							sudoku.BanujPolicko(i + 2, j + 1, k);
						}
						if (sudoku.RozmerCisla < 4) {
							for (int k = 1; k < 5 - sudoku.RozmerCisla; k++) {
								sudoku.BanujPolicko(i + 1, j + 1, k);
								sudoku.BanujPolicko(i + 1, j + 2, k);
							}
						}
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
			FunkceNapisZadani.NapisZadaniRadek(sudoku.Uloha.Rimska_Radek, sudoku.RozmerMrizka, "Rimska radek:");
			FunkceNapisZadani.NapisZadaniSloupec(sudoku.Uloha.Rimska_Sloupec, sudoku.RozmerMrizka, "Rimska sloupec:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// je-li soucet policka a sousediciho policka 10, pak sousedici policko musi byt 10 - cislo
			// je-li soucet policka a sousediciho policka 5, pak sousedici policko musi byt 5 - cislo
			// jinak jejich soucet nesmi byt 5 ci 10
			foreach (var policko in Policka[radek - 1, sloupec - 1]) {
				if (policko[2] == 10) {
					sudoku.BanujPolickoInverse(policko[0], policko[1], 10 - cislo);
				} else if (policko[2] == 5) {
					sudoku.BanujPolickoInverse(policko[0], policko[1], 5 - cislo);
				} else {
					if (cislo < 5 && 5 - cislo <= sudoku.RozmerCisla) {
						sudoku.BanujPolicko(policko[0], policko[1], 5 - cislo);
					}
					if (cislo < 10 && 10 - cislo <= sudoku.RozmerCisla) {
						sudoku.BanujPolicko(policko[0], policko[1], 10 - cislo);
					}
				}
			}
		}
	}
}
