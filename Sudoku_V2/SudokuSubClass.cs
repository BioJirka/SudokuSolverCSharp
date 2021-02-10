using System.Collections.Generic;

namespace Sudoku {
	// tridy Policko se objevuji v tabulce 9x9 a udrzuji informace jako:
		// zda je dane policko jiz vyplneno, jakym cislem, jaka cisla lze vepsat atd.
	public class Policko {
		public int Cislo;
		public int BanyPocet;
		public bool[] Bany;
		public int Hodnota;
		public int HodnotaBonus;
		public Policko(int rozmerCisla) {
			this.Bany = new bool[rozmerCisla];
		}
	}
	// trida Dalsi urcuje, jak bude hlavni program postupovat pri lusteni sudoku
	// zda bude vepisovat, menit ci mazat cisla atd.
	public class Dalsi {
		public int Radek;
		public int Sloupec;
		public int Hodnota;
		public int Indikator;
		public bool Error;
	}
	// tridy KrokBan se vyskytuji jako prvky tridy Krok
	// nesou informace, o tom, jaka cisla z jakych poli jsme banovali pri zapisu noveho cisla
	public class KrokBan {
		public int Radek;
		public int Sloupec;
		public int Cislo;
		public KrokBan(int radek, int sloupec, int cislo) {
			this.Radek = radek;
			this.Sloupec = sloupec;
			this.Cislo = cislo;
		}
	}
	// tridy Krok se budou vyskytovat jako prvky dynamickeho listu
	// budou nest informace o prubehu vyplnovani sudoku
	// slouzi zejmena pro funkci rollbacku, kdyz se dostaneme do slepe vetve
	public class Krok {
		public int Radek;
		public int Sloupec;
		public int CisloPoradi;
		public List<int> MoznaCisla;
		public List<KrokBan> KrokBany;
		public Krok(int radek, int sloupec) {
			this.Radek = radek;
			this.Sloupec = sloupec;
			this.MoznaCisla = new List<int>();
			this.KrokBany = new List<KrokBan>();
		}
	}
}