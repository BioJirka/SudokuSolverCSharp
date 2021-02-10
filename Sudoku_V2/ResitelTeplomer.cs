// pravidlo - v sudoku jsou vyznaceny teplomery
	// plati, ze cisla v teplomeru musi od banky ke spicce rust (nemusi byt po sobe jdouci)
// vstup - tabulky 9x9 nadepsane Teplomer_1, Teplomer_2
	// kazda banka teplomeru ma vepsano jedno z cisel 1, 11, 21,... (cisla se nesmi opakovat)
	// kazda dalsi cast teplomeru ma vepsano cislo o 1 vetsi, nez ta predchozi

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelTeplomer : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public int[,] PolickaPomoc;
		public int MaxTeplomer;
		public bool FlagHodnota;
		public List<List<string>> TeplomerPrint;
		public ResitelTeplomer(Sudoku sudoku) {
			// polickum v teplomeru pripiseme bonus
			this.HodnotaBonus = 500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// zjistime, jake nejvyssi cislo se na vstupu nachazi
			this.MaxTeplomer = 0;
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Teplomer_1[i][j] != "") {
						this.MaxTeplomer = Math.Max(this.MaxTeplomer, Convert.ToInt32(sudoku.Uloha.Teplomer_1[i][j]));
					}
				}
			}
			// vytvorime pomocne pole PolickaPomoc, ktere ma tolik radku, jako je nejvyssi cislo na vstupu
			// kazdy radek udava souradnice policka s cislem rovnym danemu radku
			if (this.MaxTeplomer != 0) {
				this.PolickaPomoc = new int[this.MaxTeplomer, 2];
				for (int i = 0; i < sudoku.RozmerMrizka; i++) {
					for (int j = 0; j < sudoku.RozmerMrizka; j++) {
						if (sudoku.Uloha.Teplomer_1[i][j] != "") {
							this.PolickaPomoc[Convert.ToInt32(sudoku.Uloha.Teplomer_1[i][j]) - 1, 0] = i + 1;
							this.PolickaPomoc[Convert.ToInt32(sudoku.Uloha.Teplomer_1[i][j]) - 1, 1] = j + 1;
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						}
					}
				}
				// pokud jsou cisla po sobe jdouci, znamena to, ze jsou po sobe v teplomeru
				// zapiseme jejich vzajemny odkaz (treti cislo je 1 nebo -1, dle toho, ktere policko je vetsi (1 znamena ja jsem vetsi))
				for (int i = 0; i < this.MaxTeplomer - 1; i++) {
					if (this.PolickaPomoc[i, 0] != 0 && this.PolickaPomoc[i + 1, 0] != 0) {
						this.Policka[PolickaPomoc[i, 0] - 1, this.PolickaPomoc[i, 1] - 1].Add(new int[]{this.PolickaPomoc[i + 1, 0], this.PolickaPomoc[i + 1, 1], -1});
						this.Policka[PolickaPomoc[i + 1, 0] - 1, this.PolickaPomoc[i + 1, 1] - 1].Add(new int[]{this.PolickaPomoc[i, 0], this.PolickaPomoc[i, 1], 1});
					}
				}
			}
			// provedeme totez pro tabulku Teplomer_2
			this.MaxTeplomer = 0;
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Teplomer_2[i][j] != "") {
						this.MaxTeplomer = Math.Max(this.MaxTeplomer, Convert.ToInt32(sudoku.Uloha.Teplomer_2[i][j]));
					}
				}
			}
			if (this.MaxTeplomer != 0) {
				this.PolickaPomoc = new int[this.MaxTeplomer, 2];
				for (int i = 0; i < sudoku.RozmerMrizka; i++) {
					for (int j = 0; j < sudoku.RozmerMrizka; j++) {
						if (sudoku.Uloha.Teplomer_2[i][j] != "") {
							this.PolickaPomoc[Convert.ToInt32(sudoku.Uloha.Teplomer_2[i][j]) - 1, 0] = i + 1;
							this.PolickaPomoc[Convert.ToInt32(sudoku.Uloha.Teplomer_2[i][j]) - 1, 1] = j + 1;
							sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						}
					}
				}
				for (int i = 0; i < this.MaxTeplomer - 1; i++) {
					if (this.PolickaPomoc[i, 0] != 0 && this.PolickaPomoc[i + 1, 0] != 0) {
						this.Policka[PolickaPomoc[i, 0] - 1, this.PolickaPomoc[i, 1] - 1].Add(new int[]{this.PolickaPomoc[i + 1, 0], this.PolickaPomoc[i + 1, 1], -1});
						this.Policka[PolickaPomoc[i + 1, 0] - 1, this.PolickaPomoc[i + 1, 1] - 1].Add(new int[]{this.PolickaPomoc[i, 0], this.PolickaPomoc[i, 1], 1});
					}
				}
			}
			// provedeme stejny postup pocatecniho banovani jako v ResitelNerovnost
			this.PolickaPomoc = new int[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int k = 1; k <= sudoku.RozmerCisla; k++) {
				for (int i = 0; i < sudoku.RozmerMrizka; i++) {
					for (int j = 0; j < sudoku.RozmerMrizka; j++) {
						if (this.PolickaPomoc[i, j] == 0) {
							this.FlagHodnota = true;
							foreach (var policko in this.Policka[i, j]) {
								if (policko[2] == 1 && new List<int>{0, k}.Contains(this.PolickaPomoc[policko[0] - 1, policko[1] - 1])) {
									this.FlagHodnota = false;
								}
							}
							if (this.FlagHodnota) {
								this.PolickaPomoc[i, j] = k;
							}
						}
					}
				}
			}
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					for (int k = 1; k < this.PolickaPomoc[i, j]; k++) {
						sudoku.BanujPolicko(i + 1, j + 1, k);
					}
				}
			}
			this.PolickaPomoc = new int[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int k = sudoku.RozmerCisla; k >= 1; k--) {
				for (int i = 0; i < sudoku.RozmerMrizka; i++) {
					for (int j = 0; j < sudoku.RozmerMrizka; j++) {
						if (this.PolickaPomoc[i, j] == 0) {
							this.FlagHodnota = true;
							foreach (var policko in this.Policka[i, j]) {
								if (policko[2] == -1 && new List<int>{0, k}.Contains(this.PolickaPomoc[policko[0] - 1, policko[1] - 1])) {
									this.FlagHodnota = false;
								}
							}
							if (this.FlagHodnota) {
								this.PolickaPomoc[i, j] = k;
							}
						}
					}
				}
			}
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					for (int k = this.PolickaPomoc[i, j] + 1; k <= sudoku.RozmerCisla; k++) {
						sudoku.BanujPolicko(i + 1, j + 1, k);
					}
				}
			}
			this.PolickaPomoc = null;
		}
		public void NapisZadani(Sudoku sudoku) {
			this.TeplomerPrint = new List<List<string>>();
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				this.TeplomerPrint.Add(new List<string>());
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.MaxTeplomer = 0;
					if (sudoku.Uloha.Teplomer_1[i][j] != "") {
						this.MaxTeplomer = Math.Max(this.MaxTeplomer, Convert.ToInt32(sudoku.Uloha.Teplomer_1[i][j]) % 10);
					}
					if (sudoku.Uloha.Teplomer_2[i][j] != "") {
						this.MaxTeplomer = Math.Max(this.MaxTeplomer, Convert.ToInt32(sudoku.Uloha.Teplomer_2[i][j]) % 10);
					}
					if (this.MaxTeplomer == 0) {
						this.TeplomerPrint[i].Add("");
					} else {
						this.TeplomerPrint[i].Add(Convert.ToString(this.MaxTeplomer));
					}
				}
			}
			FunkceNapisZadani.NapisZadaniPolicko(this.TeplomerPrint, sudoku.RozmerMrizka, "Teplomer:");
			this.TeplomerPrint = null;
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// je-li policko soucasti teplomeru, pak predchozi a nasledujici policko nesmi obsahovat vetsi eventualne mensi cisla
			foreach (var policko in Policka[radek - 1, sloupec - 1]) {
				if (policko[2] == 1) {
					for (int i = cislo + 1; i <= sudoku.RozmerCisla; i++) {
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
