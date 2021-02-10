using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Sudoku {
	// trida Uloha je urcena k nacteni zadani ze souboru a zakladnimu zpracovani techto dat
	public class UlohaSudoku {
		public int RozmerCisla;
		public int RozmerMrizka;
		public StreamReader Soubor;
		public List<List<string>> ObsahSoubor;
		public List<string> Typ;
		public List<string[]> TypKombinace;
		public List<string> TypPouzite;
		public List<string> TypSoubor;
		public List<int[]> SouradniceSoubor;
		public int SouradnicePomocna;
		public List<List<string>> Pomocna;
		// nasledujici plozky se vytvori jen a pouze, pokud je vyzaduje zadany typ sudoku
		public List<List<string>> Cisla;
		public List<List<string>> Region;
		public List<List<string>> Suda;
		public List<List<string>> Licha;
		public List<List<string>> Pevnost;
		public List<List<string>> Nerovnost_Radek;
		public List<List<string>> Nerovnost_Sloupec;
		public List<List<string>> Teplomer_1;
		public List<List<string>> Teplomer_2;
		public List<List<string>> Sousledna_Radek;
		public List<List<string>> Sousledna_Sloupec;
		public List<List<string>> Rimska_Radek;
		public List<List<string>> Rimska_Sloupec;
		public List<List<string>> Rozdil_Radek;
		public List<List<string>> Rozdil_Sloupec;
		public List<List<string>> KdeJe9;
		public UlohaSudoku(string adresar, string soubor, int rozmerCisla, int rozmerMrizka) {
			this.RozmerCisla = rozmerCisla;
			this.RozmerMrizka = rozmerMrizka;
			// tato polozka slouzi jako vstup a rika, jaka pole jsou pro dany typ sudoku potrebna
			this.TypKombinace = new List<string[]> {new string[] {"Basic"},
													new string[] {"Diagonal"},
													new string[] {"Windoku"},
													new string[] {"Region", "Region"},
													new string[] {"Suda", "Suda"},
													new string[] {"Licha", "Licha"},
													new string[] {"AntiJezdec"},
													new string[] {"Pevnost", "Pevnost"},
													new string[] {"Nerovnost", "Nerovnost_Radek", "Nerovnost_Sloupec"},
													new string[] {"Teplomer", "Teplomer_1", "Teplomer_2"},
													new string[] {"Sousledna", "Sousledna_Radek", "Sousledna_Sloupec"},
													new string[] {"Rimska", "Rimska_Radek", "Rimska_Sloupec"},
													new string[] {"Rozdil", "Rozdil_Radek", "Rozdil_Sloupec"},
													new string[] {"KdeJe9", "KdeJe9"}};
			// nacteme vstupni data
			this.Soubor = new StreamReader(adresar + '\\' + soubor);
			this.ObsahSoubor = new List<List<string>>();
			while (!this.Soubor.EndOfStream) {
				this.ObsahSoubor.Add(new List<string>());
				foreach (var sloupec in Soubor.ReadLine().Split(';')) {
					this.ObsahSoubor[this.ObsahSoubor.Count - 1].Add(sloupec);
				}
			}
			// zapiseme si, o jaky typ sudoku se jedna
			this.Typ = new List<string>();
			foreach (var obsah in this.ObsahSoubor[0]) {
				if (obsah != "") {
					this.Typ.Add(obsah);
				}
			}
			// vypiseme si, jake tabulky je potreba (s ohledem na typ sudoku) vytvorit
			this.TypPouzite = new List<string>{"Cisla"};
			foreach (var typ in this.TypKombinace) {
				if (this.Typ.Contains(typ[0])) {
					for (int i = 1; i < typ.Length; i++) {
						this.TypPouzite.Add(typ[i]);
					}
				}
			}
			// poznacime si, jake z tabulek, ktere je potreba vytvorit, se skutecne ve vstupnim souboru nachazeji a na jakych radcich
			this.TypSoubor = new List<string>();
			this.SouradniceSoubor = new List<int[]>();
			this.SouradnicePomocna = -1;
			for (int i = 0; i < this.ObsahSoubor.Count; i++) {
				if (this.TypPouzite.Contains(this.ObsahSoubor[i][0])) {
					this.TypSoubor.Add(this.ObsahSoubor[i][0]);
					if (this.SouradnicePomocna != -1) {
						this.SouradniceSoubor[this.SouradniceSoubor.Count - 1][1] = i;
					}
					this.SouradniceSoubor.Add(new int[] {i, 0});
					this.SouradnicePomocna = i;
				}
			}
			if (this.SouradnicePomocna != -1) {
				this.SouradniceSoubor[this.SouradniceSoubor.Count - 1][1] = this.ObsahSoubor.Count;
			}
			// vytvorime potrebne tabulky, ktere se nachazeji v souboru
			for (int k = 0; k < this.TypSoubor.Count; k++) {
				this.Pomocna = new List<List<string>>();
				for (int i = 0; i < this.RozmerMrizka; i++) {
					this.Pomocna.Add(new List<string>());
					for (int j = 0; j < this.RozmerMrizka; j++) {
						if (i < this.SouradniceSoubor[k][1] - this.SouradniceSoubor[k][0] - 1 && j < this.ObsahSoubor[this.SouradniceSoubor[k][0] + i + 1].Count) {
							this.Pomocna[i].Add(this.ObsahSoubor[this.SouradniceSoubor[k][0] + i + 1][j]);
						} else {
							this.Pomocna[i].Add("");
						}
					}
					this.GetType().GetField(this.TypSoubor[k]).SetValue(this, this.Pomocna);
				}
			}
			// vytvorime i tabulky, ktere potrebujeme, ale ve vstupnim souboru nejsou, vytvorime je jako prazdne
			foreach (var typ in this.TypPouzite) {
				if (!this.TypSoubor.Contains(typ)) {
					this.Pomocna = new List<List<string>>();
					for (int i = 0; i < this.RozmerMrizka; i++) {
						this.Pomocna.Add(new List<string>());
						for (int j = 0; j < this.RozmerMrizka; j++) {
							this.Pomocna[i].Add("");
						}
					}
					this.GetType().GetField(typ).SetValue(this, this.Pomocna);
				}
			}
			this.ObsahSoubor = null;
			this.Pomocna = null;
			this.Soubor = null;
			this.SouradniceSoubor = null;
			this.TypKombinace = null;
			this.TypPouzite = null;
			this.TypSoubor = null;
		}
	}
}
