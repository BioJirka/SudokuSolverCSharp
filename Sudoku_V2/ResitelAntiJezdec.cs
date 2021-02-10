// pravidlo - lze-li z jednoho policka na druhe skocit sachovym jezdcem, pak tato dve policka nesmi obsahovat stejne cislo
// vstup - neni

using System;
using System.Collections.Generic;

namespace Sudoku {
	public class ResitelAntiJezdec : IResitel {
		public int HodnotaBonus;
		public List<int[]>[,] Policka;
		public ResitelAntiJezdec(Sudoku sudoku) {
			// polickum pripiseme drobny bonus za kazde pole, na ktere se da jezdcem skocit
			this.HodnotaBonus = 10;
			this.Policka = new List<int[]>[sudoku.RozmerMrizka,sudoku.RozmerMrizka];
			// pro kazde policko si zapiseme, na ktera dalsi policka se da jezdcem skocit
			// potreba davat pozor na indexy, abyhom nepretekli hranici pole
			for (int i = 0; i < sudoku.RozmerMrizka; i++) {
				for (int j = 0; j < sudoku.RozmerMrizka; j++) {
					this.Policka[i, j] = new List<int[]>();
					if (i + 2 < sudoku.RozmerMrizka && j + 1 < sudoku.RozmerMrizka) {
						this.Policka[i, j].Add(new int[2] {i + 3, j + 2});
					}
					if (i + 1 < sudoku.RozmerMrizka && j + 2 < sudoku.RozmerMrizka) {
						this.Policka[i, j].Add(new int[2] {i + 2, j + 3});
					}
					if (i + 2 < sudoku.RozmerMrizka && j > 0) {
						this.Policka[i, j].Add(new int[2] {i + 3, j});
					}
					if (i + 1 < sudoku.RozmerMrizka && j > 1) {
						this.Policka[i, j].Add(new int[2] {i + 2, j - 1});
					}
					if (i > 1 && j + 1 < sudoku.RozmerMrizka) {
						this.Policka[i, j].Add(new int[2] {i - 1, j + 2});
					}
					if (i > 0 && j + 2 < sudoku.RozmerMrizka) {
						this.Policka[i, j].Add(new int[2] {i, j + 3});
					}
					if (i > 1 && j > 0) {
						this.Policka[i, j].Add(new int[2] {i - 1, j});
					}
					if (i > 0 && j > 1) {
						this.Policka[i, j].Add(new int[2] {i, j - 1});
					}
					sudoku.Policka[i, j].HodnotaBonus = Math.Max(this.HodnotaBonus * this.Policka[i, j].Count, sudoku.Policka[i, j].HodnotaBonus);
				}
			}
		}
		public void NapisZadani(Sudoku sudoku) {
		}
		public void Banuj(Sudoku sudoku, int radek, int sloupec, int cislo) {
			// dane cislo zakazame ze vsech poli, kam se da skocit jezdcem
			foreach (var policko in Policka[radek - 1, sloupec - 1]) {
				sudoku.BanujPolicko(policko[0], policko[1], cislo);
			}
		}
	}
}
