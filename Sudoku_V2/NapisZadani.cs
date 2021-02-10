// v teto staticke tride si definujeme 4 globalni funkce pro vypsani zadani
// ty budou volany pomoci trid ResitelSpecialniTyp

using System;
using System.Collections.Generic;

namespace Sudoku {
	public static class FunkceNapisZadani {
		// slouzi pro vypsani cisel/symbolu v polich mrizky 9x9 (lze i jiny rozmer, ale grafika se trochu rozsype)
		public static void NapisZadaniPolicko(List<List<string>> policka, int rozmerMrizka, string popis) {
			Console.WriteLine(popis);
			for (int i = 0; i < rozmerMrizka; i++) {
				if (i % 3 == 0) {
					Console.WriteLine("-------------");
				}
				for (int j = 0; j < rozmerMrizka; j++) {
					if (j % 3 == 0) {
						Console.Write('|');
					}
					if (policka[i][j] == "") {
						Console.Write(' ');
					} else {
						Console.Write(policka[i][j]);
					}
				}
				Console.WriteLine('|');
			}
			Console.WriteLine("-------------");
			Console.WriteLine();
		}
		// slouzi pro vypsani cisel/symbolu na pomezi dvou sousedicich poli na stejnem radku (napr. nerovnost, jejich rozdil)
		public static void NapisZadaniRadek(List<List<string>> policka, int rozmerMrizka, string popis) {
			Console.WriteLine(popis);
			for (int i = 0; i < rozmerMrizka; i++) {
				if (i % 3 == 0) {
					Console.WriteLine("----------");
				}
				Console.Write('|');
				for (int j = 0; j < rozmerMrizka - 1; j++) {
					if (policka[i][j] != "") {
						Console.Write(policka[i][j]);
					} else if (j % 3 == 2) {
						Console.Write('|');
					} else {
						Console.Write(' ');
					}
				}
				Console.WriteLine('|');
			}
			Console.WriteLine("----------");
			Console.WriteLine();
		}
		// slouzi pro vypsani cisel/symbolu na pomezi dvou sousedicich poli ve stejnem sloupci
		public static void NapisZadaniSloupec(List<List<string>> policka, int rozmerMrizka, string popis) {
			Console.WriteLine(popis);
			Console.WriteLine("-------------");
			for (int i = 0; i < rozmerMrizka - 1; i++) {
				for (int j = 0; j < rozmerMrizka; j++) {
					if (j % 3 == 0) {
						Console.Write('|');
					}
					if (policka[i][j] != "") {
						Console.Write(policka[i][j]);
					} else if (i % 3 == 2) {
						Console.Write('-');
					} else {
						Console.Write(' ');
					}
				}
				Console.WriteLine('|');
			}
			Console.WriteLine("-------------");
			Console.WriteLine();
		}
		// slouzi pro vypsani cisel/symbolu v rohu 4 sousedicich poli (napr. jejich soucet, symbol udavajici jejich vzestupne ci sestupne poradi dle hodinovych rucicek)
		public static void NapisZadaniTecka(List<List<string>> policka, int rozmerMrizka, string popis) {
			Console.WriteLine(popis);
			Console.WriteLine("----------");
			for (int i = 0; i < rozmerMrizka - 1; i++) {
				Console.Write('|');
				for (int j = 0; j < rozmerMrizka - 1; j++) {
					if (policka[i][j] != "") {
						Console.Write(policka[i][j]);
					} else if (i % 3 == 2) {
						Console.Write('-');
					} else if (j % 3 == 2) {
						Console.Write('|');
					} else {
						Console.Write(' ');
					}
				}
				Console.WriteLine('|');
			}
			Console.WriteLine("----------");
			Console.WriteLine();
		}
	}
}