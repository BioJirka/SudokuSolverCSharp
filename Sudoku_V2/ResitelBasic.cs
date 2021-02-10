// pravidlo - existuje zakladnich 9 oblasti 3x3, kde se cisla nesmi opakovat
// vstup - neni

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelBasic : IResitel {
		public List<int[]>[,] Policka;
		public List<int[][]> PolickaPomoc;
		public ResitelBasic(Sudoku sudoku) {
			this.Policka = new List<int[]>[sudoku.RozmerMrizka,sudoku.RozmerMrizka];
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
				}
			}
			// vytvorime si techto 9 oblasti
			this.PolickaPomoc = new List<int[][]>();
			this.PolickaPomoc.Add(new int[][]{new int[]{1,1},new int[]{1,2},new int[]{1,3},new int[]{2,1},new int[]{2,2},new int[]{2,3},new int[]{3,1},new int[]{3,2},new int[]{3,3}});
			this.PolickaPomoc.Add(new int[][]{new int[]{1,4},new int[]{1,5},new int[]{1,6},new int[]{2,4},new int[]{2,5},new int[]{2,6},new int[]{3,4},new int[]{3,5},new int[]{3,6}});
			this.PolickaPomoc.Add(new int[][]{new int[]{1,7},new int[]{1,8},new int[]{1,9},new int[]{2,7},new int[]{2,8},new int[]{2,9},new int[]{3,7},new int[]{3,8},new int[]{3,9}});
			this.PolickaPomoc.Add(new int[][]{new int[]{4,1},new int[]{4,2},new int[]{4,3},new int[]{5,1},new int[]{5,2},new int[]{5,3},new int[]{6,1},new int[]{6,2},new int[]{6,3}});
			this.PolickaPomoc.Add(new int[][]{new int[]{4,4},new int[]{4,5},new int[]{4,6},new int[]{5,4},new int[]{5,5},new int[]{5,6},new int[]{6,4},new int[]{6,5},new int[]{6,6}});
			this.PolickaPomoc.Add(new int[][]{new int[]{4,7},new int[]{4,8},new int[]{4,9},new int[]{5,7},new int[]{5,8},new int[]{5,9},new int[]{6,7},new int[]{6,8},new int[]{6,9}});
			this.PolickaPomoc.Add(new int[][]{new int[]{7,1},new int[]{7,2},new int[]{7,3},new int[]{8,1},new int[]{8,2},new int[]{8,3},new int[]{9,1},new int[]{9,2},new int[]{9,3}});
			this.PolickaPomoc.Add(new int[][]{new int[]{7,4},new int[]{7,5},new int[]{7,6},new int[]{8,4},new int[]{8,5},new int[]{8,6},new int[]{9,4},new int[]{9,5},new int[]{9,6}});
			this.PolickaPomoc.Add(new int[][]{new int[]{7,7},new int[]{7,8},new int[]{7,9},new int[]{8,7},new int[]{8,8},new int[]{8,9},new int[]{9,7},new int[]{9,8},new int[]{9,9}});
			// pro kazde policko si zapiseme, jaka dalsi policka se nachazeji ve stejne oblasti s nim
			foreach (var policko in this.PolickaPomoc) {
				for (int i = 0; i < policko.Length; i++) {
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
			// zakazeme dane cislo ve vsech polickach dane oblasti
			foreach (var policko in this.Policka[radek - 1, sloupec - 1]) {
				sudoku.BanujPolicko(policko[0], policko[1], cislo);
			}
		}
	}
}
