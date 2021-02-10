// pravidlo - mezi nekterymi sousedicimi policky je vyznacena nerovnost, ta musi byt dodrzena
// vstup - tabulka 9x8 nadepsana Nerovnost_Radek (obsahuje '>' nebo '<') a tabulka 8x9 nadepsana Nerovnost_Sloupec (obsahuje 'V' nebo 'A')

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelNerovnost : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public int[,] PolickaPomoc;
		public bool FlagHodnota;
		public ResitelNerovnost(Sudoku sudoku) {
			// polickum, ktere maji na nejake hrane nerovnost pripiseme bonus
			this.HodnotaBonus = 500;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// narazime-li v radku na nerovnost, pak si u techto dvou policek zapiseme vzajemny odkaz
			// treti cislo udava, ktere z techto cisel je vetsi (1 znamena ja jsem vetsi, -1 naopak)
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka - 1; j++) {
					if (sudoku.Uloha.Nerovnost_Radek[i][j] == "<") {
						this.Policka[i, j].Add(new int[] {i + 1 , j + 2, -1});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, 1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
					} else if (sudoku.Uloha.Nerovnost_Radek[i][j] == ">") {
						this.Policka[i, j].Add(new int[] {i + 1 , j + 2, 1});
						this.Policka[i, j + 1].Add(new int[] {i + 1, j + 1, -1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i, j + 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j + 1].HodnotaBonus);
					}
				}
			}
			// totez pro sloupce
			for (int i = 0; i < sudoku.RozmerMrizka - 1; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Nerovnost_Sloupec[i][j] == "A") {
						this.Policka[i, j].Add(new int[] {i + 2 , j + 1, -1});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, 1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
					} else if (sudoku.Uloha.Nerovnost_Sloupec[i][j] == "V") {
						this.Policka[i, j].Add(new int[] {i + 2 , j + 1, 1});
						this.Policka[i + 1, j].Add(new int[] {i + 1, j + 1, -1});
						sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i, j].HodnotaBonus);
						sudoku.Policka[i + 1, j].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[i + 1, j].HodnotaBonus);
					}
				}
			}
			// zde je potreba se na chvili zastavit a probrat zbyly postup v teto inicializacni funkci
        	// obdobne jako ve tride ResitelPevnost muzeme o nekterych polich rici, ze nemohou obsahovat 1 nebo 9 (dle nerovnosti)
        	// na rozdil od tridy ResitelPevnost se tyto nerovnosti mohou retezit, tedy muze platit, ze:
  	          // A < B < C < D, tedy: A not in [7,8,9], B not in [1,8,9], C not in [1,2,9], D not in [1,2,3]
    	    // takovychto retezcu muze byt v sudoku mnoho a to ruzne delky (od 2 do 9)
    	    // nebudeme tedy tyto retezce hledat, ale zvolime jiny, ekvivalentni postup:
    	        // vytvorime si pomocne pole PolickaPomoc velikosti 9x9
    	        // projdeme pole Policka a do vsech policek v pomocnem poli , kam lze zapsat cislo 1 (nejsou vetsi nez zadne jine pole) napiseme 1
    	        // postup opakujeme tentokrat ale s tim rozdilem, ze nerovnosti u poli, ktere obsahuji 1, ignorujeme, a vepisujeme 2
    	        // postup opakujeme az po cislo 9
    	        // nyni kazde cislo v pomocnem poli udava, jake nejnizsi cislo do nej muze byt zapsano
    	        // je-li tedy v policku zapsano 3, pak banujeme 1 a 2
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
			// postup opakujeme, abychom urcili, jake nejvetsi cislo muze v tom kterem policku byt
			// namisto od 1 zaciname vyplnovat od 9 a vsimame si obracenych nerovnosti
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
			FunkceNapisZadani.NapisZadaniRadek(sudoku.Uloha.Nerovnost_Radek, sudoku.RozmerMrizka, "Nerovnost radek:");
			FunkceNapisZadani.NapisZadaniSloupec(sudoku.Uloha.Nerovnost_Sloupec, sudoku.RozmerMrizka, "Nerovnost sloupec:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// je-li u policka nerovnost, pak u sousediciho policka banujeme cisla mensi ci vetsi, dle nerovnosti
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
