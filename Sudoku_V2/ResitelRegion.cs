// pravidlo - existuji nepravidelne oblasti, ve kterych se cisla nesmi opakovat (doplnuje nebo uplne nahrazuje standardnich 9 oblasti 3x3)
// vstup - tabulka 9x9 nadepsana Region
	// pro kazdy region je urcen string, ktery je vepsan do kazdeho pole daneho regionu (tyto stringy nesmi byt pro ruzne regiony stejne)

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelRegion : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public List<List<int[]>> PolickaPomoc;
		public List<string> PomocIndex;
		public ResitelRegion(Sudoku sudoku) {
			// polickum v techto specialnich regionech pripiseme drobny bonus
			this.HodnotaBonus = 100;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka, sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// nacteme data a vypiseme si jednotlive regiony do promenne PolickaPomoc
			// promenna PomocIndex slouzi k urceni
				// zda jsme se jiz se zadanym regionem setkali a tedy mame pridat policko do jiz stavajiciho listu
				// nebo je zde dany region novy a mame zalozit novy list
			this.PolickaPomoc = new List<List<int[]>>();
			this.PomocIndex = new List<string>();
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					if (sudoku.Uloha.Region[i][j] != "") {
						if (this.PomocIndex.Contains(sudoku.Uloha.Region[i][j])) {
							this.PolickaPomoc[this.PomocIndex.IndexOf(sudoku.Uloha.Region[i][j])].Add(new int[]{i + 1, j + 1});
						} else {
							this.PomocIndex.Add(sudoku.Uloha.Region[i][j]);
							this.PolickaPomoc.Add(new List<int[]>{new int[]{i + 1, j + 1}});
						}
					}
				}
			}
			// pro dane policko si zapiseme policka, ktera jsou ve stejne oblasti s nim
			foreach (var policko in this.PolickaPomoc) {
				for (int i = 0; i < policko.Count; i++) {
					sudoku.Policka[policko[i][0] - 1, policko[i][1] - 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[policko[i][0] - 1, policko[i][1] - 1].HodnotaBonus);
					for (int j = i + 1; j < policko.Count; j++) {
						this.Policka[policko[i][0] - 1, policko[i][1] - 1].Add(new int[]{policko[j][0], policko[j][1]});
						this.Policka[policko[j][0] - 1, policko[j][1] - 1].Add(new int[]{policko[i][0], policko[i][1]});
					}
				}
			}
			this.PolickaPomoc = null;
			this.PomocIndex = null;
		}
		public void NapisZadani(Sudoku sudoku) {
			FunkceNapisZadani.NapisZadaniPolicko(sudoku.Uloha.Region, sudoku.RozmerMrizka, "Region:");
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// cislo zakazeme ve vsech polickach dane oblasti
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				sudoku.BanujPolicko(policko[0], policko[1], cislo);
			}
		}
	}
}
