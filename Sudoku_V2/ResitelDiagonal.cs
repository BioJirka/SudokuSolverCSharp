// pravidlo - na dvou hlavnich diagonalach se cisla nesmi opakovat
// vstup - neni

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelDiagonal : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public List<int[][]> PolickaPomoc;
		public ResitelDiagonal(Sudoku sudoku) {
			// polickum na diagonalach pripiseme drobny bonus
			this.HodnotaBonus = 100;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka,sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// vytvorime si tyto dve diagonaly
			this.PolickaPomoc = new List<int[][]>();
			this.PolickaPomoc.Add(new int[][]{new int[]{1,1},new int[]{2,2},new int[]{3,3},new int[]{4,4},new int[]{5,5},new int[]{6,6},new int[]{7,7},new int[]{8,8},new int[]{9,9}});
			this.PolickaPomoc.Add(new int[][]{new int[]{1,9},new int[]{2,8},new int[]{3,7},new int[]{4,6},new int[]{5,5},new int[]{6,4},new int[]{7,3},new int[]{8,2},new int[]{9,1}});
			// pro kazde policko na diagonale si vypiseme, jaka dalsi policka s nim na diagonale jsou
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
			// jsme-li na diagonale (list neni prazdny), pak cislo zakazeme na zbyvajicich policka diagonaly
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				sudoku.BanujPolicko(policko[0], policko[1], cislo);
			}
		}
	}
}
