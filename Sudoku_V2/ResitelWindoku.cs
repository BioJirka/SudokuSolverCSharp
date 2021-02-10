// pravidlo - existuji 4 specialni oblasti ([2,2]:[4,4], [2,6]:[4,8], [6,2]:[8,4], [6,6]:[8,8]), ve kterych se cisla nesmi opakovat
// vstup - neni

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelWindoku : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public List<int[][]> PolickaPomoc;
		public ResitelWindoku(Sudoku sudoku) {
			// polickum v techto specialnich oblastech pripiseme drobny bonus
			this.HodnotaBonus = 100;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka,sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// vytvorime si tyto 4 specialni oblasti
			this.PolickaPomoc = new List<int[][]>();
			this.PolickaPomoc.Add(new int[][]{new int[]{2,2},new int[]{2,3},new int[]{2,4},new int[]{3,2},new int[]{3,3},new int[]{3,4},new int[]{4,2},new int[]{4,3},new int[]{4,4}});
			this.PolickaPomoc.Add(new int[][]{new int[]{2,6},new int[]{2,7},new int[]{2,8},new int[]{3,6},new int[]{3,7},new int[]{3,8},new int[]{4,6},new int[]{4,7},new int[]{4,8}});
			this.PolickaPomoc.Add(new int[][]{new int[]{6,2},new int[]{6,3},new int[]{6,4},new int[]{7,2},new int[]{7,3},new int[]{7,4},new int[]{8,2},new int[]{8,3},new int[]{8,4}});
			this.PolickaPomoc.Add(new int[][]{new int[]{6,6},new int[]{6,7},new int[]{6,8},new int[]{7,6},new int[]{7,7},new int[]{7,8},new int[]{8,6},new int[]{8,7},new int[]{8,8}});
			// pro dane policko si zapiseme, jaka dalsi policka jsou s nim v dane oblasti
			foreach (var policko in this.PolickaPomoc) {
				for (int i = 0; i < policko.Length; i++) {
					sudoku.Policka[policko[i][0] - 1, policko[i][1] - 1].HodnotaBonus = Math.Max(this.HodnotaBonus, sudoku.Policka[policko[i][0] - 1, policko[i][1] - 1].HodnotaBonus);
					for (int j = i + 1; j < policko.Length; j++) {
						this.Policka[policko[i][0] - 1, policko[i][1] - 1].Add(new int[]{policko[j][0], policko[j][1]});
						this.Policka[policko[j][0] - 1, policko[j][1] - 1].Add(new int[]{policko[i][0], policko[i][1]});
					}
				}
			}
			this.PolickaPomoc = null;
		}
		public void NapisZadani(Sudoku sudoku) {
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// cislo zakazeme ve vsech polickach v dane oblasti
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				sudoku.BanujPolicko(policko[0], policko[1], cislo);
			}
		}
	}
}
